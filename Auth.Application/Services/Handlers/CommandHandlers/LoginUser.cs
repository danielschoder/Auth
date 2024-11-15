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
            return new AuthResponse(ErrorMessage: "Email|Please provide an email.");
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return new AuthResponse(ErrorMessage: "Password|Please provide a password.");
        }
        var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
        return user is not null && _passwordHelper.PasswordIsVerified(user.PasswordHash, password) ?
            new AuthResponse(Jwt: _tokenProvider.Create(user)) :
            new AuthResponse(Unauthorized: true);
        // Update Last Login DateTime
    }
}
