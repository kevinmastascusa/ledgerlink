using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class UpdateTransactionDto
    {
        [StringLength(500)]
        public string? Description { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;
    }
} 