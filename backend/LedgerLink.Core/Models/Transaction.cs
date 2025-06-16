using System;

namespace LedgerLink.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty; // "Deposit", "Withdrawal", "Transfer", etc.
        public string Description { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string Status { get; set; } = string.Empty; // "Pending", "Completed", "Failed", etc.

        // Foreign keys
        public int AccountId { get; set; }
        public int? UserId { get; set; }

        // Navigation properties
        public virtual Account? Account { get; set; }
        public virtual User? User { get; set; }
    }
} 