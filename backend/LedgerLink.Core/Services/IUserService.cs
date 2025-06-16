using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using System.Threading.Tasks;

namespace LedgerLink.Core.Services
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(CreateUserDto createUserDto);
        Task<User?> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> ExistsAsync(string email);
    }
} 