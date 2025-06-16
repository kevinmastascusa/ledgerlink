using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class CreateAccountDto
    {
        [Required, StringLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string AccountType { get; set; } = string.Empty;

        [Required]
        public decimal Balance { get; set; }

        [Required, StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }
    }
} 