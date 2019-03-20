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
        public TransactionViewModel(BankAccount bankAccount, IEnumerable<Transaction> transactions, ApplicationUser user)
        {
            Transactions = transactions;
            User = user;
            BankAccount = bankAccount;
        }

        public DateTime PeriodStartDate => Transactions.Min(t => t.TransactionDate);
        public DateTime PeriodEndDate => Transactions.Max(t => t.TransactionDate);
        public IEnumerable<Transaction> Transactions { get; }
        public BankAccount BankAccount { get; }
    }



}