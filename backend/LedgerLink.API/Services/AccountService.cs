using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Repositories;
using LedgerLink.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.API.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _accountRepository.GetByIdAsync(id);
            });
        }

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _accountRepository.GetByAccountNumberAsync(accountNumber);
            });
        }

        public async Task<IEnumerable<Account>> GetByUserIdAsync(int userId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(userId))
                {
                    throw new Exception("User not found.");
                }

                return await _accountRepository.GetByUserIdAsync(userId);
            });
        }

        public async Task<Account> CreateAsync(CreateAccountDto createAccountDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (await _accountRepository.ExistsAsync(createAccountDto.AccountNumber))
                {
                    throw new Exception("An account with this account number already exists.");
                }

                if (!await _userRepository.ExistsByIdAsync(createAccountDto.UserId))
                {
                    throw new Exception("User not found.");
                }

                var account = new Account
                {
                    AccountNumber = createAccountDto.AccountNumber,
                    AccountType = createAccountDto.AccountType,
                    Balance = createAccountDto.Balance,
                    Currency = createAccountDto.Currency,
                    Description = createAccountDto.Description ?? string.Empty,
                    UserId = createAccountDto.UserId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _accountRepository.AddAsync(account);
                return account;
            });
        }

        public async Task<Account?> UpdateAsync(int id, UpdateAccountDto updateAccountDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var existingAccount = await _accountRepository.GetByIdAsync(id);
                if (existingAccount == null)
                {
                    throw new Exception("Account not found.");
                }

                existingAccount.AccountType = updateAccountDto.AccountType;
                existingAccount.IsActive = updateAccountDto.IsActive ?? existingAccount.IsActive;
                existingAccount.LastUpdatedAt = DateTime.UtcNow;

                _accountRepository.Update(existingAccount);
                return existingAccount;
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                {
                    return false;
                }

                if (account.Balance != 0)
                {
                    throw new Exception("Cannot delete account with non-zero balance.");
                }

                _accountRepository.Remove(account);
                return true;
            });
        }

        public async Task<bool> ExistsAsync(string accountNumber)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _accountRepository.ExistsAsync(accountNumber);
            });
        }

        public async Task<decimal> GetBalanceAsync(int accountId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                return account.Balance;
            });
        }

        public async Task<bool> UpdateBalanceAsync(int accountId, decimal amount)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                account.Balance += amount;
                account.LastUpdatedAt = DateTime.UtcNow;

                _accountRepository.Update(account);
                return true;
            });
        }
    }
} 