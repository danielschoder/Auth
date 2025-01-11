using Auth.Contracts.ExternalServices;
using Auth.Infrastructure.ExternalApis;

namespace Auth.Api.Extensions;

public static class ExternalServiceRegistrations
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddHttpClient<ISlackClient, SlackClient>(client =>
        {
            client.BaseAddress = new Uri("https://hooks.slack.com");
        });
        return services;
    }
}
