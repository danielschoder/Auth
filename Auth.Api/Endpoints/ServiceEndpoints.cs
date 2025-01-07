using Auth.Application.Services.Handlers.QueryHandlers;
using MediatR;

namespace Auth.Api.Endpoints;

public static class ServiceEndpoints
{
    public static void MapVersionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetVersionAsync);
        app.MapGet("/api", GetVersionAsync);

        static async Task<IResult> GetVersionAsync(IMediator mediator)
            => Results.Ok(await mediator.Send(new GetVersion.Query()));
    }
}
