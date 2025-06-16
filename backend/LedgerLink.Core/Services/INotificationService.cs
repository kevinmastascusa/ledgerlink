using LedgerLink.Core.DTOs;
using LedgerLink.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.Core.Services
{
    public interface INotificationService
    {
        Task<Notification?> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId);
        Task<Notification> CreateAsync(CreateNotificationDto createNotificationDto);
        Task<Notification?> MarkAsReadAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> CreateTransactionNotificationAsync(Transaction transaction);
    }
} 