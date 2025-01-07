using Auth.Application.Services.Handlers.CommandHandlers;
using Auth.Contracts.DTOs;
using MediatR;

namespace Auth.Api.Endpoints;

public static class PersonsEndpoints
{
    public static void MapPersonsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/persons/login", LoginAsync);
        app.MapPost("/persons", RegisterAsync);

        static async Task<IResult> LoginAsync(LoginDto loginDto, IMediator mediator)
        {
            var authResponse = await mediator.Send(new LoginUser.Command(loginDto));
            return authResponse.Authorized ? Results.Ok(authResponse) : Results.Unauthorized();
        }

        static async Task<IResult> RegisterAsync(RegisterDto registerDto, IMediator mediator)
            => Results.Ok(await mediator.Send(new RegisterUser.Command(registerDto)));
    }
}
