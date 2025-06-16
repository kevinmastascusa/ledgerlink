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
    public class NotificationService : INotificationService
    {
        private readonly LedgerLinkDbContext _context;

        public NotificationService(LedgerLinkDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Transaction)
                .Include(n => n.Account)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Transaction)
                .Include(n => n.Account)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.Transaction)
                .Include(n => n.Account)
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> CreateAsync(CreateNotificationDto createNotificationDto)
        {
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

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> MarkAsReadAsync(int id)
        {
            var notification = await GetByIdAsync(id);
            if (notification == null)
            {
                return null;
            }
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await GetByIdAsync(id);
            if (notification == null)
            {
                return false;
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
            if (!notifications.Any())
                return false;
            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
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
    }
} 