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
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            var accounts = GetUserAccounts();
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
    }
}