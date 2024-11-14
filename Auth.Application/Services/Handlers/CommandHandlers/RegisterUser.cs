using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.Responses;
using Auth.Domain.Common.Interfaces;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class RegisterUser(
    IUserRepository userRepository,
    IPasswordHelper passwordHelper,
    ITokenProvider tokenProvider,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<RegisterUser.Command, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public record Command(RegisterDto RegisterDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        var email = command.RegisterDto.Email?.Trim().ToLower();
        var password = command.RegisterDto.Password?.Trim();
        if (string.IsNullOrWhiteSpace(email))
        {
            return new AuthResponse("Please provide an email.", unauthorized: false);
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return new AuthResponse("Please provide a password.", unauthorized: false);
        }
        var existingUser = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            return new AuthResponse("A user with this email already exists.", unauthorized: false);
        }
        var now = _dateTimeProvider.UtcNow;
        var newUser = new User
        {
            Email = email,
            PasswordHash = _passwordHelper.HashPassword(password),
            CreatedDateTime = now,
            LastLoginDateTime = now
        };
        await _userRepository.AddUserAsync(newUser, cancellationToken);
        return new AuthResponse(_tokenProvider.Create(newUser));
    }
}

