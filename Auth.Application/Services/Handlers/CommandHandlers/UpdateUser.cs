using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.Responses;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class UpdateUser(
    IUserRepository userRepository,
    ITokenProvider tokenProvider)
    : IRequestHandler<UpdateUser.Command, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public record Command(UserUpdateDto UserUpdateDto) : IRequest<AuthResponse>;

    public async Task<AuthResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        var updateUserDto = command.UserUpdateDto with { NewEmail = command.UserUpdateDto.NewEmail?.Trim().ToLower() };
        await _userRepository.UpdateUserAsync(updateUserDto, cancellationToken);
        return new AuthResponse(new UserDto
        (
            updateUserDto.Id,
            updateUserDto.NewEmail,
            _tokenProvider.Create(updateUserDto.Id.ToString()))
        );
    }
}
