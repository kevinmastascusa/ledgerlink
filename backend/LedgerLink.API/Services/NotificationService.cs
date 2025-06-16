using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using LedgerLink.Core.Repositories;
using LedgerLink.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.API.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                return await _notificationRepository.GetByIdAsync(id);
            });
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(userId))
                {
                    throw new Exception("User not found.");
                }

                return await _notificationRepository.GetByUserIdAsync(userId);
            });
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(userId))
                {
                    throw new Exception("User not found.");
                }

                return await _notificationRepository.GetUnreadByUserIdAsync(userId);
            });
        }

        public async Task<Notification> CreateAsync(CreateNotificationDto createNotificationDto)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(createNotificationDto.UserId))
                {
                    throw new Exception("User not found.");
                }

                var notification = new Notification
                {
                    Title = createNotificationDto.Title,
                    Message = createNotificationDto.Message,
                    Type = createNotificationDto.Type,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    UserId = createNotificationDto.UserId,
                    TransactionId = createNotificationDto.TransactionId,
                    AccountId = createNotificationDto.AccountId
                };

                await _notificationRepository.AddAsync(notification);
                return notification;
            });
        }

        public async Task<Notification?> MarkAsReadAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    return null;
                }

                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                _notificationRepository.Update(notification);
                return notification;
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    return false;
                }

                _notificationRepository.Remove(notification);
                return true;
            });
        }

        public async Task<bool> DeleteAllByUserIdAsync(int userId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(userId))
                {
                    throw new Exception("User not found.");
                }

                var notifications = await _notificationRepository.GetByUserIdAsync(userId);
                foreach (var notification in notifications)
                {
                    _notificationRepository.Remove(notification);
                }
                return true;
            });
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                if (!await _userRepository.ExistsByIdAsync(userId))
                {
                    throw new Exception("User not found.");
                }

                var unreadNotifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);
                return unreadNotifications.Count();
            });
        }

        public async Task<bool> CreateTransactionNotificationAsync(Transaction transaction)
        {
            var notificationDto = new CreateNotificationDto
            {
                Title = "Transaction Completed",
                Message = $"A {transaction.TransactionType} transaction of {transaction.Amount} {transaction.Currency} has been completed.",
                Type = "Transaction",
                UserId = transaction.UserId ?? 0,
                TransactionId = transaction.Id,
                AccountId = transaction.AccountId
            };

            await CreateAsync(notificationDto);
            return true;
        }

        public async Task<bool> CreateAccountNotificationAsync(Account account, string message)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var notificationDto = new CreateNotificationDto
                {
                    Title = "Account Update",
                    Message = message,
                    Type = "Account",
                    UserId = account.UserId,
                    AccountId = account.Id
                };

                await CreateAsync(notificationDto);
                return true;
            });
        }
    }
} 