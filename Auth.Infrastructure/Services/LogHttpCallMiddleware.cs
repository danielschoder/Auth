using Auth.Domain.Common.Interfaces;
using Auth.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auth.Infrastructure.Services;

public class LogHttpCallMiddleware(
    RequestDelegate next,
    IWebHostEnvironment env)
{
    private readonly RequestDelegate _next = next;
    private readonly IWebHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var logger = httpContext.RequestServices.GetRequiredService<IScopedLogger>();

        logger.Log($"{httpContext.Request.Path.Value}{httpContext.User.GetIdAsLogString()}");

        await _next(httpContext);

        if (_env.IsDevelopment())
        {
            logger.WriteLogsToDebugOutput();
        }
    }
}
