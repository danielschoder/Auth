using Auth.Application.Interfaces;
using MediatR;

namespace Auth.Application.Services.Handlers.CommandHandlers;

public class UpdateLastLogin(
    IUserRepository userRepository)
    : INotificationHandler<UpdateLastLogin.Notification>
{
    private readonly IUserRepository _userRepository = userRepository;

    public record Notification(Guid UserId) : INotification;

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
        => await _userRepository.UpdateLastLoginAsync(notification.UserId, cancellationToken);
}
