using MediatR;

namespace Auth.Application.Extensions;

internal static class MediatrExtensions
{
    public static void PublishInBackground(
        this IMediator mediator,
        INotification notification,
        CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            await mediator.Publish(notification, cancellationToken);
        }, cancellationToken);
    }
}
