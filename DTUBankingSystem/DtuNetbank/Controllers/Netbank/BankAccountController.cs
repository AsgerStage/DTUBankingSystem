using System;
using System.Collections.Generic;
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
            //var accountJsonModels = GetAccounts();            
            //var accounts = accountJsonModels.Select(a => new BankAccount()
            //{
            //    AccountName = a.account_name, IBAN = a._id,
            //    AccountNumber = a.account_numbers.Single(i => i._type == "BBAN_DK").value, User = user,
            //    Balance = decimal.Parse(a.available_balance)
            //}).ToList();

            var accounts = GetUserAccounts();
            string status = Message == AccountMessageId.AccountAlreadyAdded? "Denne konto er allerede registreret" :
                            Message == AccountMessageId.UnableToFindAccount? "Denne konto eksistere ikke" :
                            Message == AccountMessageId.AccountSuccesfullyAdded? "Konto tilføjet!" : "";
            ViewBag.statusMessage = status;
            return View("Accounts", accounts);
        }
        public ActionResult AddAccount(string accountId)
        {
            if(accountId == null)
                RedirectToAction("Index", "BankAccount");

            var userid = User.Identity.GetUserId();
            AccountMessageId responseMsg;
            using (var db = new ApplicationDbContext())
            {
                var accounts  = db.BankAccounts.Where(a => a.UserId.ToString() == userid 
                                                        && a.AccountNumber.ToString() == accountId).ToList();
                if(accounts.Count > 0)
                    responseMsg = AccountMessageId.AccountAlreadyAdded;
                else{
                    var accountJsonModels = GetAccounts();
                    var accountsFromNordea = accountJsonModels.
                                            Where(a => a._id.ToString() == accountId).
                                            Select(a => new BankAccount()
                                            {
                                                AccountName = a.account_name, IBAN = a._id,
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
        


        private IEnumerable<BankAccount> GetUserAccounts()
        {
            var userid = User.Identity.GetUserId();
            if (string.IsNullOrWhiteSpace(userid))
                throw new Exception("User is not identified");
            using (var db = new ApplicationDbContext())
            {
                var accounts = db.BankAccounts.Include(a => a.Transactions).Where(a => a.UserId.ToString() == userid).ToArray();
                return accounts;
            }
        }


        private ICollection<BankAccountJsonModel> GetAccounts()
        {
            var nordeaApiManager = new NordeaAPIv3Manager();
            var accounts = nordeaApiManager.GetAccounts();
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