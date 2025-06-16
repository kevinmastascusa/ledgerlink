using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LedgerLink.Data.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly LedgerLinkDbContext _context;
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;

        public TransactionService(
            LedgerLinkDbContext context,
            IAccountService accountService,
            INotificationService notificationService)
        {
            _context = context;
            _accountService = accountService;
            _notificationService = notificationService;
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transaction?> GetByTransactionNumberAsync(string transactionNumber)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.User)
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.User)
                .Where(t => t.AccountId == accountId && 
                           t.TransactionDate >= startDate && 
                           t.TransactionDate <= endDate)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<Transaction> CreateAsync(CreateTransactionDto createTransactionDto)
        {
            var account = await _accountService.GetByIdAsync(createTransactionDto.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException("Account not found.");
            }

            var transaction = new Transaction
            {
                TransactionNumber = GenerateTransactionNumber(),
                Amount = createTransactionDto.Amount,
                Currency = createTransactionDto.Currency,
                TransactionType = createTransactionDto.TransactionType,
                Description = createTransactionDto.Description,
                TransactionDate = createTransactionDto.TransactionDate,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                AccountId = createTransactionDto.AccountId,
                UserId = createTransactionDto.UserId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(int id, UpdateTransactionDto updateTransactionDto)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction == null)
            {
                return null;
            }

            if (transaction.Status == "Completed")
            {
                throw new InvalidOperationException("Cannot update a completed transaction.");
            }

            transaction.Description = updateTransactionDto.Description;
            transaction.Status = updateTransactionDto.Status;
            transaction.LastUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction == null)
            {
                return false;
            }

            if (transaction.Status == "Completed")
            {
                throw new InvalidOperationException("Cannot delete a completed transaction.");
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string transactionNumber)
        {
            return await _context.Transactions.AnyAsync(t => t.TransactionNumber == transactionNumber);
        }

        public async Task<bool> ProcessTransactionAsync(Transaction transaction)
        {
            var account = await _accountService.GetByIdAsync(transaction.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException("Account not found.");
            }

            // Update account balance
            decimal amount = transaction.TransactionType == "Credit" ? transaction.Amount : -transaction.Amount;
            await _accountService.UpdateBalanceAsync(account.Id, amount);

            // Update transaction status
            transaction.Status = "Completed";
            transaction.LastUpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Create notification
            await _notificationService.CreateTransactionNotificationAsync(transaction);

            return true;
        }

        public async Task<bool> ReverseTransactionAsync(int transactionId)
        {
            var transaction = await GetByIdAsync(transactionId);
            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction not found.");
            }

            if (transaction.Status != "Completed")
            {
                throw new InvalidOperationException("Only completed transactions can be reversed.");
            }

            // Reverse the transaction amount
            decimal amount = transaction.TransactionType == "Credit" ? -transaction.Amount : transaction.Amount;
            await _accountService.UpdateBalanceAsync(transaction.AccountId, amount);

            // Update transaction status
            transaction.Status = "Reversed";
            transaction.LastUpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Create notification
            await _notificationService.CreateTransactionNotificationAsync(transaction);

            return true;
        }

        public async Task<decimal> GetTotalByAccountIdAsync(int accountId)
        {
            if (!await _context.Accounts.AnyAsync(a => a.Id == accountId))
            {
                throw new InvalidOperationException("Account not found.");
            }

            return await _context.Transactions
                .Where(t => t.AccountId == accountId && t.Status == "Completed")
                .SumAsync(t => t.TransactionType == "Credit" ? t.Amount : -t.Amount);
        }

        private string GenerateTransactionNumber()
        {
            // Generate a unique 12-digit transaction number
            var random = new Random();
            var transactionNumber = string.Empty;
            do
            {
                transactionNumber = string.Join("", Enumerable.Range(0, 12).Select(_ => random.Next(0, 10)));
            } while (_context.Transactions.Any(t => t.TransactionNumber == transactionNumber));

            return transactionNumber;
        }
    }
} 