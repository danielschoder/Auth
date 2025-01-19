using Auth.Contracts.ExternalServices;
using Auth.Infrastructure.ExternalApis;

namespace Auth.Api.Extensions;

public static class ExternalServiceRegistrations
{
    public static IServiceCollection AddExternalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<ISlackClient, SlackClient>(client =>
        {
            client.BaseAddress = new Uri($"{configuration["Slack:BaseAddress"]}");
        });

        services.AddHttpClient<IWbsToolApiClient, WbsToolApiClient>(client =>
        {
            client.BaseAddress = new Uri($"{configuration["WbsToolApi:BaseAddress"]}");
        });

        return services;
    }
}
