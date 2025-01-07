using Auth.Application.Extensions;
using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.Responses;
using Auth.Domain.Entities;
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
    private string _email;
    private string _password;
    private User _user;
    private AuthResponse _response;

    public record Command(LoginDto LoginDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        return await command.Error(
        [
            (() => FormatEmailPassword(command), null),
            (CheckEmailPassword, new AuthResponse(ErrorMessage: "Please provide an email and a password.")),
            (CheckEmail, new AuthResponse(ErrorMessage: "Please provide an email.")),
            (CheckPassword, new AuthResponse(ErrorMessage: "Please provide a password.")),
            (GetUserAsync, new AuthResponse(Authorized: false)),
            (VerifyPassword, new AuthResponse(Authorized: false)),
            (UpdateLastLoginAsync, null),
            (CreateJwt, null)
        ]) ??
        _response;

        Task<bool> FormatEmailPassword(Command command)
        {
            _email = command.LoginDto.Email?.Trim().ToLower();
            _password = command.LoginDto.Password?.Trim();
            return Task.FromResult(true);
        }

        Task<bool> CheckEmailPassword()
            => Task.FromResult(!string.IsNullOrWhiteSpace(_email) || !string.IsNullOrWhiteSpace(_password));

        Task<bool> CheckEmail()
            => Task.FromResult(!string.IsNullOrWhiteSpace(_email));

        Task<bool> CheckPassword()
            => Task.FromResult(!string.IsNullOrWhiteSpace(_password));

        async Task<bool> GetUserAsync()
        {
            _user = await _userRepository.GetByEmailAsync(_email, cancellationToken);
            return _user is not null;
        }

        Task<bool> VerifyPassword()
            => Task.FromResult(_passwordHelper.IsPasswordVerified(_user.PasswordHash, _password));

        async Task<bool> UpdateLastLoginAsync()
        {
            await _userRepository.UpdateLastLoginAsync(_user.Id, cancellationToken);
            return true;
        }

        Task<bool> CreateJwt()
        {
            _response = new AuthResponse(new UserDto(_user.Id, _email, _tokenProvider.Create(_user.Id.ToString())));
            return Task.FromResult(true);
        }
    }
}
