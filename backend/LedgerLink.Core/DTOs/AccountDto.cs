using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class AccountBalanceDto
    {
        [Required(ErrorMessage = "Account ID is required")]
        public int AccountId { get; set; }

        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters")]
        public string Currency { get; set; } = string.Empty;
    }
} 