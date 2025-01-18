using Auth.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Auth.Infrastructure.Services;

public class ExceptionServiceBase(
    IHttpContextAccessor httpContext,
    IScopedLogger scopedLogger)
{
    protected readonly IHttpContextAccessor _httpContext = httpContext;
    protected readonly IScopedLogger _scopedLogger = scopedLogger;

    protected async Task WriteResponse500Async<T>(T responseBody)
    {
        _httpContext.HttpContext.Response.StatusCode = 500;
        _httpContext.HttpContext.Response.ContentType = "application/json";
        await _httpContext.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
    }
}
