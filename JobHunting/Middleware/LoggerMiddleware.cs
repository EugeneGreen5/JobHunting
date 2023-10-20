using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace JobHunting.Middleware;

public class LoggerMiddleware
{
    private readonly ILogger<LoggerMiddleware> _logger;
    private readonly RequestDelegate _requestDelegate;

    public LoggerMiddleware(ILogger<LoggerMiddleware> logger, RequestDelegate requestDelegate)
    {
        _logger = logger;
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await Console.Out.WriteLineAsync("----------------------------------------");
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation($"Request: {context.Request.Protocol}, {context.Request.Scheme}, '{context.Request.Path}' received");
        try
        {
            await _requestDelegate(context);
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation($"Response: {context.Response.StatusCode} returned in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
