using System.Threading.Tasks;

namespace LedgerLink.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendTemplatedEmailAsync(string to, string templateName, object templateData);
        Task SendPasswordResetEmailAsync(string to, string resetToken);
        Task SendWelcomeEmailAsync(string to, string name);
        Task SendTransactionNotificationAsync(string to, string accountNumber, string transactionType, decimal amount);
    }
} 