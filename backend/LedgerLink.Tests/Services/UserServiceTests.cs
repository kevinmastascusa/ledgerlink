using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Repositories;
using LedgerLink.Core.Services;
using LedgerLink.API.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LedgerLink.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x["JwtSettings:Secret"]).Returns("your-256-bit-secret");
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("your-256-bit-secret-your-256-bit-secret-your-256-bit-secret-your-256-bit-secret");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("test-issuer");
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("test-audience");
            _mockConfiguration.Setup(x => x["JwtSettings:ExpirationInMinutes"]).Returns("60");

            _userService = new UserService(_mockUserRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Email = "test@example.com" };
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingUser_ReturnsNull()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Id = 1, Email = email };
            _mockUserRepository.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "test@example.com",
                Password = "Test123!",
                FirstName = "Test",
                LastName = "User"
            };

            var user = new User
            {
                Id = 1,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName
            };

            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createUserDto.Email, result.Email);
            Assert.Equal(createUserDto.FirstName, result.FirstName);
            Assert.Equal(createUserDto.LastName, result.LastName);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Test123!"
            };

            var user = new User
            {
                Id = 1,
                Email = loginDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password),
                IsActive = true
            };

            _mockUserRepository.Setup(x => x.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            var user = new User
            {
                Id = 1,
                Email = loginDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword")
            };

            _mockUserRepository.Setup(x => x.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
        {
            // Arrange
            var userId = 1;
            var updateUserDto = new UpdateUserDto
            {
                FirstName = "Updated",
                LastName = "User",
                PhoneNumber = "1234567890"
            };

            var existingUser = new User
            {
                Id = userId,
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            var result = await _userService.UpdateAsync(userId, updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateUserDto.FirstName, result.FirstName);
            Assert.Equal(updateUserDto.LastName, result.LastName);
            Assert.Equal(updateUserDto.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public async Task DeleteAsync_ExistingUser_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId };
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingUser_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.False(result);
        }
    }
} 