using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DtuNetbank.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string IBAN { get; set; }

        private ICollection<Transaction> Transactions { get; set; }


    }
}