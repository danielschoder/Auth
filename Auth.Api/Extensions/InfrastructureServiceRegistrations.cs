using Auth.Domain.Common.Interfaces;
using Auth.Infrastructure.Services;

namespace Auth.Api.Extensions;

public static class InfrastructureServiceRegistrations
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
