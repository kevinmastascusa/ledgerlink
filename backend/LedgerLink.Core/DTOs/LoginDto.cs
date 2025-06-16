using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class LoginDto
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
} 