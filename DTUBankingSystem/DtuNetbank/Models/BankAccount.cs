using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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
        public string Account_id { get; set; }
        [Display(Name = "Saldo")]
        public decimal Balance { get; set;}

        public ICollection<Transaction> Transactions { get; set; }
    }
    public class RegisterAccountModel
    {
     
        [Required]
        [Display(Name = "Account_id")]
        public string IBAN { get; set; }
    }

    public class BankAccountsViewModel
    {
        public ICollection<BankAccountJsonModel> BankAccounts { get; set; }
        public ApplicationUser User { get; set; }
    }


    public class BankAccountJsonModel
    {
        [Display(Name = "Country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [Display(Name = "Account Numbers")]
        [JsonProperty("account_numbers")]
        public ICollection<AccountNumber> AccountNumbers { get; set; }

        [Display(Name = "Currency")]
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [Display(Name = "Account Name")]
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [Display(Name = "Product")]
        [JsonProperty("product")]
        public string Product { get; set; }

        [Display(Name = "Account Type")]
        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [Display(Name = "Available Balance")]
        [JsonProperty("available_balance")]
        public string AvailableBalance { get; set; }

        [Display(Name = "Booked Balance")]
        [JsonProperty("booked_balance")]
        public string BookedBalance { get; set; }

        [Display(Name = "Value Dated Balance")]
        [JsonProperty("value_dated_balance")]
        public string ValueDatedBalance { get; set; }

        [Display(Name = "Bank")]
        [JsonProperty("bank")]
        public Bank Bank { get; set; }

        [Display(Name = "Status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [Display(Name = "Credit Limit")]
        [JsonProperty("credit_limit")]
        public string CreditLimit { get; set; }

        [Display(Name = "Latest Transaction Booking Date")]
        [JsonProperty("latest_transaction_booking_date")]
        public string LatestTransactionBookingDate { get; set; }

        [Display(Name = "Links")]
        [JsonProperty("_links")]
        public ICollection<Link> Links { get; set; }

        [Display(Name = "ID")]
        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public class Link
    {
        [Display(Name = "Relative")]
        [JsonProperty("rel")]
        public string Relative { get; set; }

        [Display(Name = "Hyper Reference")]
        [JsonProperty("href")]
        public string HRef { get; set; }
    }

    public class Bank
    {
        [Display(Name = "Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display(Name = "BIC")]
        [JsonProperty("bic")]
        public string BIC { get; set; }

        [Display(Name = "Country")]
        [JsonProperty("country")]
        public string Country { get; set; }

    }

    public class AccountNumber
    {
        [Display(Name = "Value")]
        [JsonProperty("value")]
        public string Value { get; set; }

        [Display(Name = "Type")]
        [JsonProperty("_type")]
        public string Type { get; set; }
    }
}