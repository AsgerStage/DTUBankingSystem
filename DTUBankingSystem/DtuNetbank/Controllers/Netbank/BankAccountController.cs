using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DtuNetbank.Models;
using Microsoft.AspNet.Identity;

namespace DtuNetbank.Controllers.Netbank
{
    public class BankAccountController : PortalController
    {
        // GET: BankAccount
        public ActionResult Index(AccountMessageId? Message)
        {
            var user = GetCurrentUser();
            var userBankAccounts = GetUserAccounts();


            var accountJsonModels = GetAccountsFromApi(new string[] { });
            var accounts = accountJsonModels.Select(a => new BankAccount()
            {
                AccountName = a.account_name,
                Account_id = a._id,
                AccountNumber = a.account_numbers.Single(i => i._type == "BBAN_DK").value,
                User = user,
                Balance = decimal.Parse(a.available_balance)
            }).ToList();
            return View("Accounts", accounts);
        }
        
    
        //// GET: BankAccount
        //public ActionResult Index(AccountMessageId? Message)
        //{
        //    var user = GetCurrentUser();
        //    //var accountJsonModels = GetAccountsFromApi();            
        //    //var accounts = accountJsonModels.Select(a => new BankAccount()
        //    //{
        //    //    AccountName = a.account_name, Account_id = a._id,
        //    //    AccountNumber = a.account_numbers.Single(i => i._type == "BBAN_DK").value, User = user,
        //    //    Balance = decimal.Parse(a.available_balance)
        //    //}).ToList();

        //    var accounts = GetUserAccounts();
        //    string status = Message == AccountMessageId.AccountAlreadyAdded? "Denne konto er allerede registreret" :
        //                    Message == AccountMessageId.UnableToFindAccount? "Denne konto eksistere ikke" :
        //                    Message == AccountMessageId.AccountSuccesfullyAdded? "Konto tilføjet!" : "";
        //    ViewBag.statusMessage = status;
        //    return View("Accounts", accounts);
        //}
        public ActionResult AddAccount(string accountId)
        {
            if(accountId == null)
                RedirectToAction("Index", "BankAccount");

            var userid = User.Identity.GetUserId();
            AccountMessageId responseMsg;
            using (var db = new ApplicationDbContext())
            {
                var accounts  = db.BankAccounts.Where(a => a.User.Id == userid 
                                                        && a.AccountNumber.ToString() == accountId).ToList();
                if(accounts.Count() > 0)
                    responseMsg = AccountMessageId.AccountAlreadyAdded;
                else{
                    var accountJsonModels = GetAccountsFromApi(new string[] { });
                    var accountsFromNordea = accountJsonModels.
                                            Where(a => a._id.ToString() == accountId).
                                            Select(a => new BankAccount()
                                            {
                                                AccountName = a.account_name, Account_id = a._id,
                                                AccountNumber = a.account_numbers.Single(i => i._type == "BBAN_DK").value,
                                                User = GetCurrentUser(), Balance = decimal.Parse(a.available_balance),
                                            }).
                                            ToList();
                    if(accountsFromNordea.Count == 0)
                        responseMsg = AccountMessageId.UnableToFindAccount;
                    else{
                        db.Set<BankAccount>().Add(accountsFromNordea.ElementAt(0));
                        db.SaveChanges();
                        responseMsg = AccountMessageId.AccountSuccesfullyAdded;
                    }
                }
            }
            return RedirectToAction("Index", "BankAccount" , new { Message = responseMsg });
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


        private IEnumerable<BankAccount> GetUserAccounts()
        {
            var userid = GetCurrentUser().Id;
            if (string.IsNullOrWhiteSpace(userid))
                throw new Exception("User is not identified");
            using (var db = new ApplicationDbContext())
            {
                var accounts = db.BankAccounts.Where(a => a.UserId.ToString() == userid).ToArray();
                return accounts;
            }
        }

        private void GetAccountInfoFromApi(ICollection<BankAccount> accounts)
        {
            var accountsFromApi = GetAccountsFromApi(accounts.Select(a => a.Account_id));
            foreach (var bankAccount in accounts)
            {
                var apiAccount = accountsFromApi.SingleOrDefault(a => a._id == bankAccount.Account_id);
                if (apiAccount != null)
                {
                    bankAccount.AccountName = apiAccount.account_name;
                    bankAccount.AccountNumber = apiAccount.account_numbers.First().value;
                }
            }
        }


        private ICollection<BankAccountJsonModel> GetAccountsFromApi(IEnumerable<string> filterBy)
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var usersBankAccounts = GetUserAccounts();
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

        public ActionResult Transactions(string accountId, DateTime? startDate, DateTime? endDate)
        {
            var user = GetCurrentUser();
            var trxEndDate = endDate ?? DateTime.Today;
            var trxStartDate = startDate ?? trxEndDate.AddMonths(-3);
            var jsonTransactions = new NordeaAPIv3Manager().GetTransactions(accountId, trxStartDate, trxEndDate);
            
            var model = new TransactionViewModel(accountId, jsonTransactions, user);
            return View(model);
        }

        public enum AccountMessageId
        {
            AccountSuccesfullyAdded,
            UnableToFindAccount,
            AccountAlreadyAdded
        }

    }
}