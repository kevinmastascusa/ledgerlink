using System.Diagnostics;
using LedgerLink.Core.Services;

namespace LedgerLink.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public LoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();

        try
        {
            // Log request
            await _loggingService.LogInformationAsync(
                $"Request {requestId} started: {context.Request.Method} {context.Request.Path}");

            // Log request headers
            var headers = context.Request.Headers
                .Where(h => !h.Key.StartsWith("Authorization"))
                .ToDictionary(h => h.Key, h => h.Value.ToString());
            await _loggingService.LogDebugAsync(
                $"Request {requestId} headers: {string.Join(", ", headers.Select(h => $"{h.Key}: {h.Value}"))}");

            // Continue with the request pipeline
            await _next(context);

            stopwatch.Stop();

            // Log response
            await _loggingService.LogInformationAsync(
                $"Request {requestId} completed: {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            await _loggingService.LogErrorAsync(ex, 
                $"Request {requestId} failed: {ex.Message}");
            throw;
        }
    }
} 