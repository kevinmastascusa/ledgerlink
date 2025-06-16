using LedgerLink.Core.DTOs;
using LedgerLink.Core.Extensions;
using LedgerLink.Core.Models;
using LedgerLink.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.API.Controllers
{
    /// <summary>
    /// Controller for managing notification-related operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Gets a notification by its ID
        /// </summary>
        /// <param name="id">The ID of the notification to retrieve</param>
        /// <returns>The notification if found, NotFound if not found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetById(int id)
        {
            try
            {
                var notification = await _notificationService.GetByIdAsync(id);
                if (notification == null)
                    return NotFound();

                return Ok(notification.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of notifications for the user</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByUserId(int userId)
        {
            try
            {
                var notifications = await _notificationService.GetByUserIdAsync(userId);
                return Ok(notifications.Select(n => n.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all unread notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of unread notifications for the user</returns>
        [HttpGet("user/{userId}/unread")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUnreadByUserId(int userId)
        {
            try
            {
                var notifications = await _notificationService.GetUnreadByUserIdAsync(userId);
                return Ok(notifications.Select(n => n.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the count of unread notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>The count of unread notifications</returns>
        [HttpGet("user/{userId}/unread/count")]
        public async Task<ActionResult<int>> GetUnreadCount(int userId)
        {
            try
            {
                var count = await _notificationService.GetUnreadCountAsync(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="createNotificationDto">The notification data to create</param>
        /// <returns>The created notification</returns>
        [HttpPost]
        public async Task<ActionResult<NotificationDto>> Create(CreateNotificationDto createNotificationDto)
        {
            try
            {
                var notification = await _notificationService.CreateAsync(createNotificationDto);
                return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Marks a notification as read
        /// </summary>
        /// <param name="id">The ID of the notification to mark as read</param>
        /// <returns>The updated notification if successful, NotFound if not found</returns>
        [HttpPut("{id}/read")]
        public async Task<ActionResult<NotificationDto>> MarkAsRead(int id)
        {
            try
            {
                var notification = await _notificationService.MarkAsReadAsync(id);
                if (notification == null)
                    return NotFound();

                return Ok(notification.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a notification
        /// </summary>
        /// <param name="id">The ID of the notification to delete</param>
        /// <returns>NoContent if successful, NotFound if not found</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _notificationService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes all notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteAllByUserId(int userId)
        {
            try
            {
                await _notificationService.DeleteAllByUserIdAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 