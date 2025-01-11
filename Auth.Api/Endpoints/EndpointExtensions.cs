using MediatR;
using System.Security.Claims;

namespace Auth.Api.Endpoints
{
    public static class EndpointExtensions
    {
        public static async Task<IResult> SendQueryAsync<TResponse>(
            this IMediator mediator,
            IRequest<TResponse> query,
            CancellationToken cancellationToken = default)
            where TResponse : class
        {
            var result = await mediator.Send(query, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }

        public static Guid GetId(this ClaimsPrincipal user)
            => Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
