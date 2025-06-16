using System;

namespace LedgerLink.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Transaction", "Account", "System", etc.
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public int? TransactionId { get; set; }
        public int? AccountId { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public virtual Account? Account { get; set; }
    }
} 