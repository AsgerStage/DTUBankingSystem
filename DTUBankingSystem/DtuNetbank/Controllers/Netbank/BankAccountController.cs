using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DtuNetbank.Models;
using DtuNetbank.Models.Payments;

namespace DtuNetbank.Controllers.Netbank
{
    public class BankAccountController : PortalController
    {
        // GET: BankAccount
        public ActionResult Index()
        {
            SetContextCulture();
            var accountJsonModels = GetAccountsFromApi();
            var viewModel = new BankAccountsViewModel()
            {
                BankAccounts = accountJsonModels,
                User = GetCurrentUser()
            };
            return View(viewModel);
        }


        public ActionResult RegisterBankAccount()
        {
            SetContextCulture();
            var user = GetCurrentUser();

            var model = new BankAccount(){User = user };
            return View("Register", model);
        }

        [HttpPost]
        public ActionResult RegisterBankAccount(BankAccount model)
        {
            var user = GetCurrentUser();

            if (user.Id.Equals(model.UserId.ToString()))
            {
                AddBankAccount(model);
            }

            return RedirectToAction("Index");
        }

        private bool AddBankAccount(BankAccount model)
        {
            SetContextCulture();
            if (string.IsNullOrWhiteSpace(model.Account_id)) return false;
            using (var db = new ApplicationDbContext())
            {
                db.BankAccounts.Add(model);
                db.SaveChanges();
                return true;
            }
        }


        private IEnumerable<BankAccount> GetUserAccounts(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var accounts = db.BankAccounts.Where(a => a.UserId.ToString() == userId).ToArray();
                return accounts;
            }
        }

        private ICollection<BankAccountJsonModel> GetAccountsFromApi()
        {
            var userId = GetCurrentUser().Id;
            var nordeaApiManager = new NordeaAPIv3Manager();
            var usersBankAccounts = GetUserAccounts(userId);
            List<BankAccountJsonModel> accounts = new List<BankAccountJsonModel>();
            foreach (var BankAccount in usersBankAccounts)
            {
                var apiAccount = nordeaApiManager.GetAccountByAccountId(BankAccount.Account_id);
                if (apiAccount != null)
                {
                    accounts.Add(apiAccount);
                }
            }

            return accounts;
        }

        public ActionResult Transactions(string accountId, DateTime? startDate, DateTime? endDate, string continuationKey = "")
        {
            SetContextCulture();
            var user = GetCurrentUser();
            if (!UserCanAccessAccountData(accountId))
            {
                throw new UnauthorizedAccessException();
            }
            var maxDateValue = DateTime.Today.AddDays(-1);

            var trxEndDate = endDate ?? maxDateValue;
            var trxStartDate = startDate ?? trxEndDate.AddMonths(-3);

            // To ensure none of dates are from future
            if (trxEndDate > maxDateValue) trxEndDate = maxDateValue;
            if (trxStartDate > trxEndDate) trxStartDate = trxEndDate;

            var apiManager = new NordeaAPIv3Manager();
            var apiResponse = apiManager.GetTransactions(accountId, trxStartDate, trxEndDate, continuationKey, apiManager.AccessToken);
            
            var model = new TransactionViewModel(accountId, apiResponse.Transactions, user) {
                StartDate = trxStartDate,
                EndDate = trxEndDate,
                ContinuationKey = apiResponse.ContinuationKey};
            return View(model);
        }


        /// <summary>
        /// Compares the requested accountID with entries saved in database
        /// </summary>
        protected bool UserCanAccessAccountData(string accountId)
        {
            var userBankAccounts = GetAccountsFromApi();
            return userBankAccounts.Any(a => a.Id == accountId || a.AccountNumbers.Any(an => an.Value == accountId));
        }


        #region Payments

        public ActionResult Payment()
        {
            SetContextCulture();
            var accounts = GetAccountsFromApi();
            var viewModel = new PaymentViewModel() {
                PaymentModel = new Payment(),
                AccountSelectorItems = CreateAccountSelectorItems(accounts),
                Creditor = new Creditor { Account = new Account { Currency = "DKK", Value = "20301544117544", Type = "BBAN_DK" }, Message = "Test Own Message" },
                Debtor = new Debtor { Account = new Account { Currency = "DKK", Value = "20301544118028", Type = "BBAN_DK" }, Message = "Test Debtor Message" },
                Amount = 10.0M
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Payment(PaymentViewModel model)
        {
            var creditorAccount = model.Creditor.Account.Value;

            if (!UserCanAccessAccountData(creditorAccount))
            {
                throw new Exception("Not Allowed");
            }
            var apiManager = new NordeaAPIv3Manager();
            var apiResult = apiManager.InitiatePayment(model.Creditor, model.Debtor, model.Amount, "DKK");

            SetContextCulture();
            return View("PaymentStatus", apiResult);
        }

        public ActionResult ConfirmPayment(string id)
        {
            var apiManager = new NordeaAPIv3Manager();
            var apiResult = apiManager.ConfirmPayment(id);
            SetContextCulture();
            return RedirectToAction("PaymentStatus", routeValues: new { id = apiResult.Id });
        }

        public ActionResult PaymentStatus(string id)
        {
            var apiManager = new NordeaAPIv3Manager();
            var apiResult = apiManager.GetPayment(id);
            SetContextCulture();
            return View(apiResult);
        }

        private ICollection<SelectListItem> CreateAccountSelectorItems(ICollection<BankAccountJsonModel> accounts)
        {
            var list = new List<SelectListItem>();
            foreach(var account in accounts)
            {
                var accountNumber = account.AccountNumbers.First().Value;
                list.Add(new SelectListItem { Text = $"{account.AccountName} - {accountNumber}" , Value = accountNumber});
            }
            return list;
        }
        #endregion
    }
}