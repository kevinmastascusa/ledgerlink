using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Services;
using LedgerLink.Data.Repositories;
using LedgerLink.Data.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using LedgerLink.Core.Repositories;
using System.Linq;

namespace LedgerLink.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly IAccountService _accountService;

        public AccountServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _accountService = new AccountService(_mockAccountRepository.Object, _mockUserRepository.Object, _mockValidationService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingAccount_ReturnsAccount()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, AccountNumber = "ACC001", AccountType = "Checking" };
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetByIdAsync(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountId, result.Id);
            Assert.Equal(account.AccountNumber, result.AccountNumber);
            Assert.Equal(account.AccountType, result.AccountType);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingAccount_ReturnsNull()
        {
            // Arrange
            var accountId = 1;
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<LedgerLink.Core.Exceptions.NotFoundException>(() => _accountService.GetByIdAsync(accountId));
        }

        [Fact]
        public async Task GetByUserIdAsync_ExistingAccounts_ReturnsAccounts()
        {
            // Arrange
            var userId = 1;
            var accounts = new List<Account>
            {
                new Account { Id = 1, AccountNumber = "ACC001", AccountType = "Checking", UserId = userId },
                new Account { Id = 2, AccountNumber = "ACC002", AccountType = "Savings", UserId = userId }
            };
            _mockAccountRepository.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(accounts);

            // Act
            var result = await _accountService.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result.ToList(), a => Assert.Equal(userId, a.UserId));
        }

        [Fact]
        public async Task CreateAsync_ValidAccount_ReturnsCreatedAccount()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                AccountNumber = "ACC001",
                AccountType = "Savings",
                Balance = 1000,
                Currency = "USD",
                UserId = 1
            };

            var user = new User { Id = createAccountDto.UserId };

            _mockValidationService.Setup(x => x.ValidateAsync(It.IsAny<CreateAccountDto>())).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(x => x.GetByIdAsync(createAccountDto.UserId)).ReturnsAsync(user);
            _mockAccountRepository.Setup(x => x.AddAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            // Act
            var result = await _accountService.CreateAsync(createAccountDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createAccountDto.AccountNumber, result.AccountNumber);
            Assert.Equal(createAccountDto.AccountType, result.AccountType);
            Assert.Equal(createAccountDto.Balance, result.Balance);
            Assert.Equal(createAccountDto.Currency, result.Currency);
            Assert.Equal(createAccountDto.UserId, result.UserId);
        }

        [Fact]
        public async Task UpdateAsync_ExistingAccount_ReturnsUpdatedAccount()
        {
            // Arrange
            var accountId = 1;
            var updateAccountDto = new UpdateAccountDto
            {
                AccountType = "Investment",
                IsActive = false
            };

            var existingAccount = new Account
            {
                Id = accountId,
                AccountNumber = "ACC001",
                AccountType = "Checking",
                IsActive = true
            };

            _mockValidationService.Setup(x => x.ValidateAsync(It.IsAny<UpdateAccountDto>())).Returns(Task.CompletedTask);
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(existingAccount);

            // Act
            var result = await _accountService.UpdateAsync(accountId, updateAccountDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateAccountDto.AccountType, result.AccountType);
            Assert.Equal(updateAccountDto.IsActive, result.IsActive);
        }

        [Fact]
        public async Task DeleteAsync_ExistingAccount_ReturnsTrue()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = 0 };
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _accountService.DeleteAsync(accountId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingAccount_ReturnsFalse()
        {
            // Arrange
            var accountId = 1;
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _accountService.DeleteAsync(accountId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetBalanceAsync_ExistingAccount_ReturnsBalance()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = 1000 };
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetBalanceAsync(accountId);

            // Assert
            Assert.Equal(account.Balance, result);
        }

        [Fact]
        public async Task UpdateBalanceAsync_ExistingAccount_ReturnsTrue()
        {
            // Arrange
            var accountId = 1;
            var amount = 500;
            var account = new Account { Id = accountId, Balance = 1000 };
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);
            _mockAccountRepository.Setup(x => x.UpdateBalanceAsync(accountId, amount)).ReturnsAsync(true);

            // Act
            var result = await _accountService.UpdateBalanceAsync(accountId, amount);

            // Assert
            Assert.True(result);
        }
    }
} 