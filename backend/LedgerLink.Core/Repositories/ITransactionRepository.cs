using LedgerLink.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.Core.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId);
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Transaction?> GetByTransactionNumberAsync(string transactionNumber);
        Task<bool> ExistsAsync(string transactionNumber);
    }
} 