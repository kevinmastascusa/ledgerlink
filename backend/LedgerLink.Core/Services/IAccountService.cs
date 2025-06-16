using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.Core.Services
{
    public interface IAccountService
    {
        Task<Account?> GetByIdAsync(int id);
        Task<IEnumerable<Account>> GetByUserIdAsync(int userId);
        Task<Account> CreateAsync(CreateAccountDto createAccountDto);
        Task<Account?> UpdateAsync(int id, UpdateAccountDto updateAccountDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(string accountNumber);
        Task<decimal> GetBalanceAsync(int accountId);
        Task<bool> UpdateBalanceAsync(int accountId, decimal amount);
    }
} 