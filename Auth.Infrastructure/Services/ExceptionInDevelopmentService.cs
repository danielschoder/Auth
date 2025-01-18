using Auth.Application.Interfaces;
using Auth.Contracts.Responses;
using Auth.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Auth.Infrastructure.Services;

public class ExceptionInDevelopmentService(
    IHttpContextAccessor httpContext,
    IScopedLogger scopedLogger)
    : ExceptionServiceBase(httpContext, scopedLogger), IExceptionService
{
    public async Task HandleExceptionAsync(string message)
    {
        await WriteResponse500Async(new ExceptionResponse(message, _scopedLogger.LogQueueAsList));

        Debug.WriteLine(string.Empty);
        Debug.WriteLine("== ERROR ==");
        Debug.WriteLine(message);
        Debug.WriteLine("-----------");
        _scopedLogger.LogQueueToDebugWriteLines();
        Debug.WriteLine("===========");
        Debug.WriteLine(string.Empty);
    }
}
