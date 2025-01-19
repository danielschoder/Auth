using Auth.Api.Extensions;
using Auth.Application.Services.Handlers.CommandHandlers;
using Auth.Contracts.DTOs;
using MediatR;
using System.Security.Claims;

namespace Auth.Api.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login", LoginAsync);
        app.MapPost("/users", RegisterAsync);
        app.MapPut("/users", UpdateUserAsync).RequireAuthorization();

        static async Task<IResult> LoginAsync(LoginDto loginDto, IMediator mediator)
        {
            var authResponse = await mediator.Send(new LoginUser.Command(loginDto));
            return authResponse.Authorized ? Results.Ok(authResponse) : Results.Unauthorized();
        }

        static async Task<IResult> RegisterAsync(RegisterDto registerDto, IMediator mediator)
            => Results.Ok(await mediator.Send(new RegisterUser.Command(registerDto)));

        static async Task<IResult> UpdateUserAsync(
            HttpContext httpContext,
            ClaimsPrincipal user,
            UserUpdateDto userUpdateDto,
            IMediator mediator)
            => Results.Ok(await mediator.Send(new UpdateUser.Command(user.GetId(), userUpdateDto, httpContext.GetJwtToken())));
    }
}
