namespace SchoolManagementSystem.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        // Log informasi request yang masuk
        _logger.LogInformation(
            "Incoming Request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        // Lanjutkan ke middleware selanjutnya
        await _next(context);

        var duration = DateTime.UtcNow - startTime;

        // Log informasi response yang keluar
        _logger.LogInformation(
            "Outgoing Response: {StatusCode} | Duration: {DurationMs}ms",
            context.Response.StatusCode,
            duration.TotalMilliseconds
        );
    }
}