using LedgerLink.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.Core.Repositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<IEnumerable<Account>> GetByUserIdAsync(int userId);
        Task<Account?> GetByAccountNumberAsync(string accountNumber);
        Task<bool> ExistsAsync(string accountNumber);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> UpdateBalanceAsync(int accountId, decimal amount);
    }
} 