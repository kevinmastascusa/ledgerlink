using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class CreateNotificationDto
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Type { get; set; } = string.Empty;

        public int UserId { get; set; }
        public int? TransactionId { get; set; }
        public int? AccountId { get; set; }
    }
} 