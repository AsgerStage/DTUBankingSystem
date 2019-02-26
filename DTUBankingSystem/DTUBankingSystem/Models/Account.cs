using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTUBankingSystem.Models
{
    public class Account
    {
        public string Country { get; set; }
        public AccountNumber[] AccountNumbers { get; set; }
        public string Currency { get; set; }
        public string AccountName { get; set; }
        public string Product { get; set; }
        public string AccountType { get; set; }
        public string AvailableBalance { get; set; }
        public string BookedBalance { get; set; }
        public string ValueDatedBalance { get; set; }
        public string CreditLimit { get; set; }
        public string LatestTransactionBookingDate { get; set; }
        public Bank Bank { get; set; }
        public string Status { get; set; }
        public string ID { get; }
        public Link[] Links { get; set; }
        public Account(string ID) { this.ID = ID; }

    }
}