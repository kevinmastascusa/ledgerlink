using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Services;
using LedgerLink.Core.Exceptions;
using LedgerLink.Core.Interfaces;
using LedgerLink.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LedgerLink.Data.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;

    public AccountService(
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        IValidationService validationService)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _validationService = validationService;
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
        {
            throw new NotFoundException($"Account with ID {id} not found.");
        }
        return account;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _accountRepository.GetAllAsync();
    }

    public async Task<Account> CreateAsync(CreateAccountDto createAccountDto)
    {
        await _validationService.ValidateAsync(createAccountDto);

        var user = await _userRepository.GetByIdAsync(createAccountDto.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {createAccountDto.UserId} not found.");
        }

        var account = new Account
        {
            AccountNumber = createAccountDto.AccountNumber,
            AccountType = createAccountDto.AccountType,
            Balance = createAccountDto.Balance,
            UserId = createAccountDto.UserId,
            IsActive = true,
            Currency = createAccountDto.Currency
        };

        await _accountRepository.AddAsync(account);
        return account;
    }

    public async Task<Account?> UpdateAsync(int id, UpdateAccountDto updateAccountDto)
    {
        await _validationService.ValidateAsync(updateAccountDto);

        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
        {
            throw new NotFoundException($"Account with ID {id} not found.");
        }

        account.AccountType = updateAccountDto.AccountType;
        account.IsActive = updateAccountDto.IsActive ?? account.IsActive;

        _accountRepository.Update(account);
        return account;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
        {
            return false;
        }

        _accountRepository.Remove(account);
        return true;
    }

    public async Task<Account> GetByAccountNumberAsync(string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
        {
            throw new NotFoundException($"Account with number {accountNumber} not found.");
        }
        return account;
    }

    public async Task<IEnumerable<Account>> GetByUserIdAsync(int userId)
    {
        return await _accountRepository.GetByUserIdAsync(userId);
    }

    public async Task<bool> ExistsAsync(string accountNumber)
    {
        return await _accountRepository.ExistsAsync(accountNumber);
    }

    public async Task<decimal> GetBalanceAsync(int accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null)
        {
            throw new NotFoundException($"Account with ID {accountId} not found.");
        }

        return account.Balance;
    }

    public async Task<bool> UpdateBalanceAsync(int accountId, decimal amount)
    {
        return await _accountRepository.UpdateBalanceAsync(accountId, amount);
    }
} 