using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTUBankingSystem.Models
{
    public class Transaction
    {
        private long Id { get; set; }
        private DateTime TransactionDate { get; set; }
        private long AccountNumber { get; set; }
        public decimal TransactionAmount { get; set; }
        
    }
}