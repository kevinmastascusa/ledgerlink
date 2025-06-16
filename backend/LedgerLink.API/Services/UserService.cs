using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Repositories;
using LedgerLink.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace LedgerLink.API.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _userRepository.GetByIdAsync(id);
            });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _userRepository.GetByEmailAsync(email);
            });
        }

        public async Task<User> CreateAsync(CreateUserDto createUserDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (await _userRepository.ExistsAsync(createUserDto.Email))
                {
                    throw new Exception("A user with this email already exists.");
                }

                var user = new User
                {
                    Email = createUserDto.Email,
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    PhoneNumber = createUserDto.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Role = "User"
                };

                await _userRepository.AddAsync(user);
                return user;
            });
        }

        public async Task<User?> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var existingUser = await _userRepository.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return null;
                }

                existingUser.FirstName = updateUserDto.FirstName ?? existingUser.FirstName;
                existingUser.LastName = updateUserDto.LastName ?? existingUser.LastName;
                existingUser.PhoneNumber = updateUserDto.PhoneNumber ?? existingUser.PhoneNumber;
                existingUser.Role = updateUserDto.Role ?? existingUser.Role;

                _userRepository.Update(existingUser);
                return existingUser;
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return false;
                }

                _userRepository.Remove(user);
                return true;
            });
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var user = await _userRepository.GetByEmailAsync(changePasswordDto.Email);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    throw new Exception("Current password is incorrect.");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                _userRepository.Update(user);
                return true;
            });
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    throw new Exception("Invalid email or password.");
                }

                if (!user.IsActive)
                {
                    throw new Exception("User account is inactive.");
                }

                user.LastLoginAt = DateTime.UtcNow;
                _userRepository.Update(user);

                var token = GenerateJwtToken(user);
                return new LoginResponseDto
                {
                    Token = token
                };
            });
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _userRepository.ExistsAsync(email);
            });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not found")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 