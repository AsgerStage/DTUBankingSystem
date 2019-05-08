using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DtuNetbank.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Display(Name = "Kontonummer")]
        public string AccountNumber { get; set; }
        [Display(Name = "Konto beskrivelse")]
        public string AccountName { get; set; }
        [Display(Name = "ID")]
        public string IBAN { get; set; }
        [Display(Name = "Saldo")]
        public decimal Balance { get; set;}

        public ICollection<Transaction> Transactions { get; set; }
    }
    public class RegisterAccountModel
    {
     
        [Required]
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }
    }

    public class BankAccountJsonModel
    {
        public string country { get; set; }
        public ICollection<AccountNumber> account_numbers { get; set; }
        public string currency { get; set; }
        public string account_name { get; set; }
        public string product { get; set; }
        public string account_type { get; set; }
        public string available_balance { get; set; }
        public string booked_balance { get; set; }
        public string value_dated_balance { get; set; }
        public Bank bank { get; set; }
        public string status { get; set; }
        public string credit_limit { get; set; }
        public string latest_transaction_booking_date { get; set; }
        public ICollection<Link> _links { get; set; }
        public string _id { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Bank
    {
        public string name { get; set; }
        public string bic { get; set; }
        public string country { get; set; }

    }

    public class AccountNumber
    {
        public string value { get; set; }
        public string _type { get; set; }
    }
}