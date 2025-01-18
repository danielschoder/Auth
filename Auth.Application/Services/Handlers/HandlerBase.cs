using Auth.Domain.Common.Interfaces;
using System.Runtime.CompilerServices;

namespace Auth.Application.Services.Handlers;

public abstract class HandlerBase(IScopedLogger logger)
{
    protected readonly IScopedLogger Logger = logger;

    protected void Log(
        string message = default,
        [CallerFilePath] string callerFilePath = default,
        [CallerLineNumber] int callerLineNumber = default)
        => Logger.Log(message, callerFilePath, callerLineNumber);
}
