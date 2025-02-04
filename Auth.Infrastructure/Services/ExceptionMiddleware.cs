﻿using Auth.Application.Interfaces;
using Auth.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Auth.Infrastructure.Services;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            var message = $"{exception.Message} - {exception.InnerException?.Message ?? string.Empty}";
            var logger = httpContext.RequestServices.GetRequiredService<IScopedLogger>();
            (var className, var lineNumber) = GetStackTraceFirstLine(exception.StackTrace);
            logger.Log(message, className, lineNumber);

            var exceptionService = httpContext.RequestServices.GetRequiredService<IExceptionService>();
            await exceptionService.HandleExceptionAsync($"{className} {lineNumber}{$": {message}"}");

            static (string className, int lineNumber) GetStackTraceFirstLine(string stackTrace)
            {
                string className = string.Empty;
                int lineNumber = 0;
                var firstStackTraceLine = stackTrace.Split("\r\n").Where(t => t.Contains("Auth.")).FirstOrDefault();

                if (firstStackTraceLine is not null)
                {
                    Match match = Regex.Match(firstStackTraceLine, @"in (.*?):line (\d+)");
                    if (match.Success)
                    {
                        className = Path.GetFileNameWithoutExtension(match.Groups[1].Value);
                        lineNumber = int.Parse(match.Groups[2].Value);
                    }
                }

                return (className, lineNumber);
            }
        }
    }
}
