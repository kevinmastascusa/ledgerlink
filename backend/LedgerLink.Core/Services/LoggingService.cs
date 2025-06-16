using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LedgerLink.Core.Services
{
    public interface ILoggingService
    {
        Task LogInformationAsync(string message, params object[] args);
        Task LogWarningAsync(string message, params object[] args);
        Task LogErrorAsync(Exception ex, string message, params object[] args);
        Task LogDebugAsync(string message, params object[] args);
    }

    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public async Task LogInformationAsync(string message, params object[] args)
        {
            await Task.Run(() => _logger.LogInformation(message, args));
        }

        public async Task LogWarningAsync(string message, params object[] args)
        {
            await Task.Run(() => _logger.LogWarning(message, args));
        }

        public async Task LogErrorAsync(Exception ex, string message, params object[] args)
        {
            await Task.Run(() => _logger.LogError(ex, message, args));
        }

        public async Task LogDebugAsync(string message, params object[] args)
        {
            await Task.Run(() => _logger.LogDebug(message, args));
        }
    }
} 