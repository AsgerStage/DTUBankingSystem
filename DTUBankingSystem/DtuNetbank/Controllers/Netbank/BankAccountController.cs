﻿using System;
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
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            var accountJsonModels = GetAccounts();
            var accounts = accountJsonModels.Select(a => new BankAccount()
            {
                AccountName = a.account_name, IBAN = a._id,
                AccountNumber = a.account_numbers.Single(i => i._type == "BBAN_DK").value, User = user
            }).ToList();
            return View("Accounts", accounts);
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

        public ActionResult Transactions(string accountId)
        {
            var user = GetCurrentUser();
            var jsonTransactions = new NordeaAPIv3Manager().GetTransactions(accountId, DateTime.Today.AddMonths(-3), DateTime.Today);
            
            var model = new TransactionViewModel(accountId, jsonTransactions,user);
            return View(model);
        }

    }
}