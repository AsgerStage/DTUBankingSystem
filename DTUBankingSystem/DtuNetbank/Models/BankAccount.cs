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
        [Display(Name = "AccountNumber", ResourceType = typeof(Properties.Resources))]
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
        [Display(Name = "Country", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("country")]
        public string Country { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("account_numbers")]
        public ICollection<AccountNumber> AccountNumbers { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("product")]
        public string Product { get; set; }

        [Display(Name = "AccountType", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [Display(Name = "AvailableBalance", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("available_balance")]
        public decimal AvailableBalance { get; set; }

        [Display(Name = "BookedBalance", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("booked_balance")]
        public decimal BookedBalance { get; set; }

        [Display(Name = "ValueDatedBalance", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("value_dated_balance")]
        public decimal ValueDatedBalance { get; set; }

        [Display(Name = "Bank")]
        [JsonProperty("bank")]
        public Bank Bank { get; set; }

        [Display(Name = "Status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [Display(Name = "CreditLimit", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("credit_limit")]
        public decimal CreditLimit { get; set; }

        [Display(Name = "LatestTransactionBookingDate", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("latest_transaction_booking_date")]
        public DateTime LatestTransactionBookingDate { get; set; }

        [Display(Name = "Link")]
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

        [Display(Name = "href")]
        [JsonProperty("href")]
        public string HRef { get; set; }
    }

    public class Bank
    {
        [Display(Name = "Name", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display(Name = "BIC")]
        [JsonProperty("bic")]
        public string BIC { get; set; }

        [Display(Name = "Country", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("country")]
        public string Country { get; set; }

    }

    public class AccountNumber
    {
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("value")]
        public string Value { get; set; }

        [Display(Name = "Type")]
        [JsonProperty("_type")]
        public string Type { get; set; }
    }
}