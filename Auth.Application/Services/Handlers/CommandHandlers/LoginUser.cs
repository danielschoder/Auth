using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.Responses;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class LoginUser(
    IUserRepository userRepository,
    IPasswordHelper passwordHelper,
    ITokenProvider tokenProvider)
    : IRequestHandler<LoginUser.Command, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public record Command(LoginDto LoginDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        var error = command.LoginDto.ValidateCredentials();
        if (error is not null)
        {
            return new AuthResponse(ErrorMessage: error);
        }
        var user = await _userRepository.GetUserByEmailAsync(command.LoginDto.Email, cancellationToken);
        if (user is null || !_passwordHelper.PasswordIsVerified(user.PasswordHash, command.LoginDto.Password))
        {
            return new AuthResponse(Authorized: false);
        }
        // Update Last Login DateTime
        return new AuthResponse(Jwt: _tokenProvider.Create(user));
    }
}
