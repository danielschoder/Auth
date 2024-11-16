using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.ExternalServices;
using Auth.Contracts.Responses;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class RegisterUser(
    IUserRepository userRepository,
    IPasswordHelper passwordHelper,
    ITokenProvider tokenProvider,
    ISlackClient slackClient)
    : IRequestHandler<RegisterUser.Command, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ISlackClient _slackClient = slackClient;

    public record Command(RegisterDto RegisterDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        var error = command.RegisterDto.Validate();
        if (error is not null)
        {
            return new AuthResponse(ErrorMessage: error);
        }
        var existingUser = await _userRepository.GetByEmailAsync(command.RegisterDto.Email, cancellationToken);
        if (existingUser is not null)
        {
            return new AuthResponse(ErrorMessage: "A user with this email already exists.");
        }
        var newUser = new User
        {
            Email = command.RegisterDto.Email,
            PasswordHash = _passwordHelper.HashPassword(command.RegisterDto.Password),
        };
        await _userRepository.AddAsync(newUser, cancellationToken);
        _slackClient.SendMessage(newUser.Email);
        return new AuthResponse(Jwt: _tokenProvider.Create(newUser));
    }
}
