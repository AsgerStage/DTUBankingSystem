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
            var user = GetCurrentUser();
            if (!UserCanAccessAccountData(user.Id, accountId))
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
        protected bool UserCanAccessAccountData(string userId, string accountId)
        {
            var userBankAccounts = GetUserAccounts(userId);
            return userBankAccounts.Select(a => a.Account_id).Contains(accountId);
        }


        public ActionResult Payment()
        {
            var user = GetCurrentUser();
            var accounts = GetAccountsFromApi();

            var viewModel = new PaymentViewModel() {
                PaymentModel = new Payment(),
                AccountSelectorItems = CreateAccountSelectorItems(accounts),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Payment(Payment model)
        {
            var user = GetCurrentUser();
            var accounts = GetAccountsFromApi();
            var viewModel = new PaymentViewModel()
            {
                PaymentModel = new Payment(),
                AccountSelectorItems = CreateAccountSelectorItems(accounts),
            };
            return PaymentStatus(model.Id);
        }

        public ActionResult PaymentStatus(string id)
        {
            return View();
        }

        private ICollection<SelectListItem> CreateAccountSelectorItems(ICollection<BankAccountJsonModel> accounts)
        {
            var list = new List<SelectListItem>();
            foreach(var account in accounts)
            {
                var accountNumber = account.AccountNumbers.First().Value;
                list.Add(new SelectListItem { Text = $"{accountNumber} {account.AccountName}" , Value = accountNumber});
            }
            return list;
        }

    }
}