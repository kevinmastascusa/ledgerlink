using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class UpdateAccountDto
    {
        [StringLength(50)]
        public string? AccountType { get; set; }

        public decimal? Balance { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string? Currency { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
} 