using System;

namespace LedgerLink.Core.Interfaces
{
    public interface ILoggerService
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
        void LogDebug(string message, params object[] args);
        void LogTrace(string message, params object[] args);
        void LogCritical(Exception ex, string message, params object[] args);
    }
} 