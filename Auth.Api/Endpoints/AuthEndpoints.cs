using Auth.Application.Services.Handlers.CommandHandlers;
using Auth.Contracts.DTOs;
using MediatR;

namespace Auth.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/login", LoginAsync);
        app.MapPost("/api/register", RegisterAsync);

        static async Task<IResult> LoginAsync(LoginDto loginDto, IMediator mediator)
        {
            var authResponse = await mediator.Send(new LoginUser.Command(loginDto));
            return authResponse.Authorized ? Results.Ok(authResponse) : Results.Unauthorized();
        }

        static async Task<IResult> RegisterAsync(RegisterDto registerDto, IMediator mediator)
            => Results.Ok(await mediator.Send(new RegisterUser.Command(registerDto)));
    }
}
