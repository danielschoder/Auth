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
        var email = command.LoginDto.Email?.Trim().ToLower();
        var password = command.LoginDto.Password?.Trim();
        if (string.IsNullOrWhiteSpace(email))
        {
            return new AuthResponse("Please provide an email.", unauthorized: false);
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return new AuthResponse("Please provide a password.", unauthorized: false);
        }
        var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            return new AuthResponse("Invalid credentials.", unauthorized: true);
        }
        if (!_passwordHelper.PasswordIsVerified(user.PasswordHash, password))
        {
            return new AuthResponse("Invalid credentials.", unauthorized: true);
        };
        return new AuthResponse(_tokenProvider.Create(user));
    }
}
