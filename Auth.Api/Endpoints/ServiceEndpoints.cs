using Auth.Application.Services.Handlers.QueryHandlers;
using Auth.Domain.Common.Interfaces;
using MediatR;

namespace Auth.Api.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetVersionAsync);
        app.MapGet("/api", GetVersionAsync);
        app.MapPost("/exceptions", ThrowError).RequireAuthorization();

        static async Task<IResult> GetVersionAsync(IMediator mediator)
            => Results.Ok(await mediator.Send(new GetVersion.Query()));

        static IResult ThrowError(IScopedLogger scopedLogger)
        {
            scopedLogger.Log("Before error");
            var zero = 0;
            var y = 1 / zero;
            scopedLogger.Log("After error");
            return Results.Ok();
        }
    }
}
