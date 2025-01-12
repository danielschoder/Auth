using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class UpdateUser(IUserRepository userRepository)
    : IRequestHandler<UpdateUser.Command, UserUpdateDto>
{
    private readonly IUserRepository _userRepository = userRepository;

    public record Command(Guid Id, UserUpdateDto UserUpdateDto) : IRequest<UserUpdateDto>;

    public async Task<UserUpdateDto> Handle(Command command, CancellationToken cancellationToken)
    {
        // TODO: Check if user with the new email already exists, if yes, return error
        var updateUserDto = command.UserUpdateDto with
        {
            Email = command.UserUpdateDto.Email?.Trim().ToLower(),
            Name = command.UserUpdateDto.Name?.Trim(),
            NickName = command.UserUpdateDto.NickName?.Trim(),
        };
        await _userRepository.UpdateUserAsync(command.Id, updateUserDto, cancellationToken);
        return updateUserDto;
    }
}
