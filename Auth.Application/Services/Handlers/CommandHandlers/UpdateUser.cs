using Auth.Application.Extensions;
using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Contracts.ExternalServices;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using Auth.Domain.Common.Interfaces;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class UpdateUser(
    IUserRepository userRepository,
    IWbsToolApiClient wbsToolApiClient,
    IScopedLogger logger)
    : HandlerBase(logger), IRequestHandler<UpdateUser.Command, UserUpdateResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IWbsToolApiClient _wbsToolApiClient = wbsToolApiClient;
    private UserUpdateRequest _userUpdateRequest;
    private UserUpdateResponse _userUpdateResponse;

    public record Command(Guid Id, UserUpdateRequest UserUpdateRequest, string Jwt) : IRequest<UserUpdateResponse>;

    public async Task<UserUpdateResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        Log($"command.Id: [{command.Id}]");
        return await command.Error(
            [
                (() => FormatUserData(command), null),
                (CheckEmail, new UserUpdateResponse(ErrorMessage: "Please provide an email.")),
                (UpdateUserAsync, null),
                (UpdatePersonAsync, null)
            ]) ??
            _userUpdateResponse;

        Task<bool> FormatUserData(Command command)
        {
            _userUpdateRequest = command.UserUpdateRequest with
            {
                Email = command.UserUpdateRequest.Email?.Trim().ToLower(),
                Name = command.UserUpdateRequest.Name?.Trim(),
                NickName = command.UserUpdateRequest.NickName?.Trim(),
            };
            return Task.FromResult(true);
        }

        Task<bool> CheckEmail()
            => Task.FromResult(!string.IsNullOrWhiteSpace(_userUpdateRequest.Email));

        async Task<bool> UpdateUserAsync()
            => await _userRepository.UpdateUserAsync(command.Id, _userUpdateRequest, cancellationToken);

        async Task<bool> UpdatePersonAsync()
        {
            _userUpdateResponse = new UserUpdateResponse(_userUpdateRequest);
            var personDto = new PersonDto(command.Id, _userUpdateRequest.Email, _userUpdateRequest.Name, _userUpdateRequest.NickName);
            return await _wbsToolApiClient.UpdatePersonAsync(personDto, command.Jwt);
        }
    }
}
