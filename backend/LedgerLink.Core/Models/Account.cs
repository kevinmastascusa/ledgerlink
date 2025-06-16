using System;
using System.Collections.Generic;

namespace LedgerLink.Core.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty; // "Checking", "Savings", "Investment", etc.
        public decimal Balance { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;

        // Foreign keys
        public int UserId { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
} 