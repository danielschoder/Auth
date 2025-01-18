using Auth.Application.Interfaces;
using Auth.Contracts.ExternalServices;
using Auth.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Auth.Infrastructure.Services;

public class ExceptionInProductionService(
    IHttpContextAccessor httpContext,
    IScopedLogger scopedLogger,
    ISlackClient slackClient)
    : ExceptionServiceBase(httpContext, scopedLogger), IExceptionService
{
    private readonly ISlackClient _slackClient = slackClient;

    public async Task HandleExceptionAsync(string message)
    {
        await WriteResponse500Async(new { });

        _slackClient.SendMessage($"{SlackExceptionHeader()}{SlackLogQueue()}");

        string SlackExceptionHeader()
            => $":boom: EXCEPTION: {message}\r\n";

        string SlackLogQueue()
            => $"{_scopedLogger.ToString()}";
    }
}
