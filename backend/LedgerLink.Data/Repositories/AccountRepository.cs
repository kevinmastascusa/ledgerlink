using LedgerLink.Core.Models;
using LedgerLink.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(LedgerLinkDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Account>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<bool> ExistsAsync(string accountNumber)
        {
            return await _dbSet.AnyAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _dbSet.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> UpdateBalanceAsync(int accountId, decimal amount)
        {
            var account = await _dbSet.FindAsync(accountId);
            if (account == null)
            {
                return false;
            }

            account.Balance += amount;
            account.LastUpdatedAt = System.DateTime.UtcNow;
            _dbSet.Update(account);
            return true;
        }
    }
} 