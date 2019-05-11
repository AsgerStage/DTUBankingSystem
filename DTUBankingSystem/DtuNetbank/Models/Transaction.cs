using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public string continuation_key { get; set; }
        public ICollection<TransactionJsonModel> transactions { get; set; }
        public ICollection<Link> _links { get; set; }
    }

    public class TransactionJsonModel
    {
        public string _type { get; set; }
        public string transaction_id { get; set; }
        public string currency { get; set; }
        public string booking_date { get; set; }
        public string value_date { get; set; }
        public string type_description { get; set; }
        public string narrative { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string own_message { get; set; }
        public string counterparty_name { get; set; }
        public string transaction_date { get; set; }
        public string card_number { get; set; }
        public string payment_date { get; set; }
        public string amount { get; set; }

}




}