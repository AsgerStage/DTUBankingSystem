using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTUBankingSystem.Backend;
using DTUBankingSystem.Models;

namespace DTUBankingSystem.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            Nordea api = new Nordea();
            Account[] accs = api.getAllAccounts();
            ViewBag.Content = accs;

            return View();
        }
    }
}