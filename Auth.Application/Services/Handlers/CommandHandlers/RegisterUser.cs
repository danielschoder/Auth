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
            return new AuthResponse(ErrorMessage: "Email|Please provide an email.");
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return new AuthResponse(ErrorMessage: "Password|Please provide a password.");
        }
        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            return new AuthResponse(ErrorMessage: "A user with this email already exists.");
        }
        var now = _dateTimeProvider.UtcNow;
        var newUser = new User
        {
            Email = email,
            PasswordHash = _passwordHelper.HashPassword(password),
            CreatedDateTime = now,
            LastLoginDateTime = now
        };
        await _userRepository.AddAsync(newUser, cancellationToken);
        return new AuthResponse(Jwt: _tokenProvider.Create(newUser));
    }
}
