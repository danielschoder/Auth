using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.Responses;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class LoginUser(
    IMediator mediator,
    IUserRepository userRepository,
    IPasswordHelper passwordHelper,
    ITokenProvider tokenProvider)
    : IRequestHandler<LoginUser.Command, AuthResponse>
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public record Command(LoginDto LoginDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        var error = command.LoginDto.Validate();
        if (error is not null)
        {
            return new AuthResponse(ErrorMessage: error);
        }
        var user = await _userRepository.GetByEmailAsync(command.LoginDto.Email, cancellationToken);
        if (user is null || !_passwordHelper.PasswordIsVerified(user.PasswordHash, command.LoginDto.Password))
        {
            return new AuthResponse(Authorized: false);
        }
        await _mediator.Publish(new UpdateLastLogin.Notification(user.Id), cancellationToken);
        return new AuthResponse(Jwt: _tokenProvider.Create(user));
    }
}
