using Auth.Contracts.Responses;
using Auth.Domain.Common.Interfaces;
using MediatR;
using System.Reflection;

namespace Auth.Application.Services.Handlers.QueryHandlers;

public class GetVersion(
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<GetVersion.Query, AliveResponse>
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public record Query() : IRequest<AliveResponse>;

    public Task<AliveResponse> Handle(Query query, CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetEntryAssembly().GetName();
        var alive = new AliveResponse(assembly.Name, _dateTimeProvider.UtcNow, assembly.Version.ToString());
        return Task.FromResult(alive);
    }
}
