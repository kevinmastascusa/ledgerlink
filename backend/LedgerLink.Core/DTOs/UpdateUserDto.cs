using System.ComponentModel.DataAnnotations;

namespace LedgerLink.Core.DTOs
{
    public class UpdateUserDto
    {
        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone, StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string? Role { get; set; }
    }
} 