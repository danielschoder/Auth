using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.ExternalServices;
using Auth.Domain.Common.Interfaces;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class UpdateUser(
    IUserRepository userRepository,
    IWbsToolApiClient wbsToolApiClient,
    IScopedLogger logger)
    : HandlerBase(logger), IRequestHandler<UpdateUser.Command, UserUpdateDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IWbsToolApiClient _wbsToolApiClient = wbsToolApiClient;

    public record Command(Guid Id, UserUpdateDto UserUpdateDto, string Jwt) : IRequest<UserUpdateDto>;

    public async Task<UserUpdateDto> Handle(Command command, CancellationToken cancellationToken)
    {
        Log($"command.Id: [{command.Id}]");

        // TODO: Check if user with the new email already exists, if yes, return error

        var updateUserDto = command.UserUpdateDto with
        {
            Email = command.UserUpdateDto.Email?.Trim().ToLower(),
            Name = command.UserUpdateDto.Name?.Trim(),
            NickName = command.UserUpdateDto.NickName?.Trim(),
        };
        await _userRepository.UpdateUserAsync(command.Id, updateUserDto, cancellationToken);

        var personDto = new PersonDto(command.Id, updateUserDto.Email, updateUserDto.Name, updateUserDto.NickName);
        await _wbsToolApiClient.UpdatePersonAsync(personDto, command.Jwt);

        return updateUserDto;
    }
}
