using Auth.Application.Extensions;
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
    private string _email;
    private string _password;
    private string _passwordHash;
    private User _newUser;
    private AuthResponse _response;

    public record Command(RegisterDto RegisterDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
        => await command.Error(
        [
            (() => FormatEmailPassword(command), null),
            (CheckEmailPassword, new AuthResponse(ErrorMessage: "Please provide an email and a password.")),
            (CheckEmail, new AuthResponse(ErrorMessage: "Please provide an email.")),
            (CheckPassword, new AuthResponse(ErrorMessage: "Please provide a password.")),
            (GetUserAsync, new AuthResponse(ErrorMessage: "A user with this email already exists.")),
            (CreatePasswordHash, null),
            (AddUserAsync, null),
            (SendNewUserNotification, null),
            (CreateJwt, null)
        ]) ??
        _response;

    private Task<bool> FormatEmailPassword(Command command)
    {
        _email = command.RegisterDto.Email?.Trim().ToLower();
        _password = command.RegisterDto.Password;
        return Task.FromResult(true);
    }

    private Task<bool> CheckEmailPassword()
        => Task.FromResult(!string.IsNullOrWhiteSpace(_email) || !string.IsNullOrWhiteSpace(_password));

    private Task<bool> CheckEmail()
        => Task.FromResult(!string.IsNullOrWhiteSpace(_email));

    private Task<bool> CheckPassword()
        => Task.FromResult(!string.IsNullOrWhiteSpace(_password));

    private async Task<bool> GetUserAsync()
    {
        _newUser = await _userRepository.GetByEmailAsync(_email);
        return _newUser is null;
    }

    private Task<bool> CreatePasswordHash()
    {
        _passwordHash = _passwordHelper.HashPassword(_password);
        return Task.FromResult(true);
    }

    private async Task<bool> AddUserAsync()
    {
        _newUser = new User
        {
            Email = _email,
            PasswordHash = _passwordHash
        };
        await _userRepository.AddAsync(_newUser); ;
        return true;
    }

    private Task<bool> SendNewUserNotification()
    {
        _slackClient.SendMessage($"New registration: {_email}");
        return Task.FromResult(true);
    }

    private Task<bool> CreateJwt()
    {
        _response = new AuthResponse(Jwt: _tokenProvider.Create(_newUser));
        return Task.FromResult(true);
    }
}
