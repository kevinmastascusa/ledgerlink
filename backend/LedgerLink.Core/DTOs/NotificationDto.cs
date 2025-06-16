using System;

namespace LedgerLink.Core.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public int UserId { get; set; }
        public int? TransactionId { get; set; }
        public int? AccountId { get; set; }
    }
} 