using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("Transaktions Beløb")]
        [Required]
        public decimal TransactionAmount { get; set; }

    }
}