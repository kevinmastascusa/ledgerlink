using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.Core.Services
{
    public interface ITransactionService
    {
        Task<Transaction?> GetByIdAsync(int id);
        Task<Transaction> GetByTransactionNumberAsync(string transactionNumber);
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId);
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate);
        Task<Transaction> CreateAsync(CreateTransactionDto createTransactionDto);
        Task<Transaction?> UpdateAsync(int id, UpdateTransactionDto updateTransactionDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(string transactionNumber);
        Task<bool> ProcessTransactionAsync(Transaction transaction);
        Task<bool> ReverseTransactionAsync(int transactionId);
        Task<decimal> GetTotalByAccountIdAsync(int accountId);
    }
} 