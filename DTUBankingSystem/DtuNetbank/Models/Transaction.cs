using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DtuNetbank.Models
{
    public class Transaction
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid BankAccountId { get; set; }
        [DisplayName("Bank konto")]
        [Required]
        public BankAccount Account { get; set; }

        [DisplayName("Transaktions Dato")]
        [Required]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Konto Nummer")]
        [Required]
        public long AccountNumber { get; set; }

        /// <summary>
        /// Transaction Ammount
        /// </summary>
        [DisplayName("Transaktions Beløb")]
        [Required]
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// Transaction Description
        /// </summary>
        public string Description { get; set; }
    }


    public class TransactionViewModel
    {
        public ApplicationUser User { get; }
        public TransactionViewModel(string accountNumber, IEnumerable<TransactionJsonModel> transactions, ApplicationUser user)
        {
            AccountNumber = accountNumber;
            Transactions = transactions;
            User = user;
        }
        public string ContinuationKey { get; set; }
        public IEnumerable<TransactionJsonModel> Transactions { get; }
        public string AccountNumber { get; set; }
        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }
    }

    public class TransactionResponseJsonModel
    {
        [Display(Name = "Continuation Key")]
        [JsonProperty("continuation_key")]
        public string ContinuationKey { get; set; }

        [Display(Name = "Transactions", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("transactions")]
        public ICollection<TransactionJsonModel> Transactions { get; set; }

        [Display(Name = "Links")]
        [JsonProperty("_links")]
        public ICollection<Link> Links { get; set; }
    }

    public class TransactionJsonModel
    {
        [Display(Name = "Type")]
        [JsonProperty("_type")]
        public string Type { get; set; }

        [Display(Name = "TransactionId", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [Display(Name = "BookingDate", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("booking_date")]
        public string BookingDate { get; set; }

        [Display(Name = "ValueDate", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("value_date")]
        public string ValueDate { get; set; }

        [Display(Name = "TypeDescription", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }

        [Display(Name = "Narrative", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("narrative")]
        public string Narrative { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("message")]
        public string Message { get; set; }

        [Display(Name = "Status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [Display(Name = "Reference", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [Display(Name = "OwnMessage", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("own_message")]
        public string OwnMessage { get; set; }

        [Display(Name = "CounterpartyName", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("counterparty_name")]
        public string CounterpartyName { get; set; }

        [Display(Name = "TransactionDate", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("transaction_date")]
        public string TransactionDate { get; set; }

        [Display(Name = "CardNumber", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [Display(Name = "PaymentDate", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("payment_date")]
        public string PaymentDate { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Properties.Resources))]
        [JsonProperty("amount")]
        public string Amount { get; set; }

    }
}