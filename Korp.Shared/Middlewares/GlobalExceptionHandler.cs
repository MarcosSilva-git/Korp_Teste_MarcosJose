using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Korp.Shared.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new ProblemDetails
        {
            Detail = "An unexpected error occurred. Please try again later.",
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Extensions = {
                ["traceId"] = context.TraceIdentifier
            }
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response),
            cancellationToken
        );

        return true;
    }
}
