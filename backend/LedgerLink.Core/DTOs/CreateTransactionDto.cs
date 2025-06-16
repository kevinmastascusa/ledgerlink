using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class CreateTransactionDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required, StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
} 