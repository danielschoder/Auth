using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class UpdateUser(
    IUserRepository userRepository)
    : IRequestHandler<UpdateUser.Command, Unit>
{
    private readonly IUserRepository _userRepository = userRepository;

    public record Command(UserUpdateDto UserUpdateDto) : IRequest<Unit>;

    public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
    {
        var updateUserDto = command.UserUpdateDto with { NewEmail = command.UserUpdateDto.NewEmail?.Trim().ToLower() };
        await _userRepository.UpdateUserAsync(updateUserDto, cancellationToken);
        return Unit.Value;
    }
}
