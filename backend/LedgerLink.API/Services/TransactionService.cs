using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Repositories;
using LedgerLink.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LedgerLink.API.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _transactionRepository.GetByIdAsync(id);
            });
        }

        public async Task<Transaction> GetByTransactionNumberAsync(string transactionNumber)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _transactionRepository.GetByTransactionNumberAsync(transactionNumber);
            });
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _accountRepository.ExistsByIdAsync(accountId))
                {
                    throw new Exception("Account not found.");
                }

                return await _transactionRepository.GetByAccountIdAsync(accountId);
            });
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(userId))
                {
                    throw new Exception("User not found.");
                }

                return await _transactionRepository.GetByUserIdAsync(userId);
            });
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _accountRepository.ExistsByIdAsync(accountId))
                {
                    throw new Exception("Account not found.");
                }

                return await _transactionRepository.GetByDateRangeAsync(startDate, endDate);
            });
        }

        public async Task<Transaction> CreateAsync(CreateTransactionDto createTransactionDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _accountRepository.ExistsByIdAsync(createTransactionDto.AccountId))
                {
                    throw new Exception("Account not found.");
                }

                if (!await _userRepository.ExistsByIdAsync(createTransactionDto.UserId))
                {
                    throw new Exception("User not found.");
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

                await _transactionRepository.AddAsync(transaction);
                return transaction;
            });
        }

        public async Task<Transaction?> UpdateAsync(int id, UpdateTransactionDto updateTransactionDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var existingTransaction = await _transactionRepository.GetByIdAsync(id);
                if (existingTransaction == null)
                {
                    return null;
                }

                if (existingTransaction.Status == "Completed")
                {
                    throw new Exception("Cannot update a completed transaction.");
                }

                existingTransaction.Description = updateTransactionDto.Description;
                existingTransaction.Status = updateTransactionDto.Status;
                existingTransaction.LastUpdatedAt = DateTime.UtcNow;

                _transactionRepository.Update(existingTransaction);
                return existingTransaction;
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);
                if (transaction == null)
                {
                    return false;
                }

                if (transaction.Status == "Completed")
                {
                    throw new Exception("Cannot delete a completed transaction.");
                }

                _transactionRepository.Remove(transaction);
                return true;
            });
        }

        public async Task<bool> ExistsAsync(string transactionNumber)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _transactionRepository.ExistsAsync(transactionNumber);
            });
        }

        public async Task<bool> ProcessTransactionAsync(Transaction transaction)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var account = await _accountRepository.GetByIdAsync(transaction.AccountId);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                // Update account balance
                decimal amount = transaction.TransactionType == "Credit" ? transaction.Amount : -transaction.Amount;
                await _accountRepository.UpdateBalanceAsync(account.Id, amount);

                // Update transaction status
                transaction.Status = "Completed";
                _transactionRepository.Update(transaction);

                // Create notification
                await _notificationService.CreateTransactionNotificationAsync(transaction);

                return true;
            });
        }

        public async Task<bool> ReverseTransactionAsync(int transactionId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var transaction = await _transactionRepository.GetByIdAsync(transactionId);
                if (transaction == null)
                {
                    throw new Exception("Transaction not found.");
                }

                if (transaction.Status != "Completed")
                {
                    throw new Exception("Only completed transactions can be reversed.");
                }

                // Reverse the transaction amount
                decimal amount = transaction.TransactionType == "Credit" ? -transaction.Amount : transaction.Amount;
                await _accountRepository.UpdateBalanceAsync(transaction.AccountId, amount);

                // Update transaction status
                transaction.Status = "Reversed";
                _transactionRepository.Update(transaction);

                // Create notification
                await _notificationService.CreateTransactionNotificationAsync(transaction);

                return true;
            });
        }

        public async Task<decimal> GetTotalByAccountIdAsync(int accountId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _accountRepository.ExistsByIdAsync(accountId))
                {
                    throw new Exception("Account not found.");
                }

                var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);
                return transactions
                    .Where(t => t.Status == "Completed")
                    .Sum(t => t.TransactionType == "Credit" ? t.Amount : -t.Amount);
            });
        }

        private string GenerateTransactionNumber()
        {
            // Generate a unique 12-digit transaction number
            var random = new Random();
            var transactionNumber = string.Empty;
            do
            {
                transactionNumber = string.Join("", Enumerable.Range(0, 12).Select(_ => random.Next(0, 10)));
            } while (_transactionRepository.ExistsAsync(transactionNumber).Result);

            return transactionNumber;
        }
    }
} 