using Auth.Application.Services;

namespace Auth.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IVersionService, VersionService>();
        return services;
    }
}
