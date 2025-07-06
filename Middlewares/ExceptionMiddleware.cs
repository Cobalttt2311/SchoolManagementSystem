using System.Net;
using System.Text.Json;

namespace SchoolManagementSystem.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Panggil middleware selanjutnya di dalam pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log error yang terjadi
            _logger.LogError(ex, "An unhandled exception has occurred.");

            // Siapkan response error
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Buat response body yang standar
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An internal server error has occurred. Please try again later.",
                // Tampilkan detail error hanya di environment Development untuk keamanan
                Detailed = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}