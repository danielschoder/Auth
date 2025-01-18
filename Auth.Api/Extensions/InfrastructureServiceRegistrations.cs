using Auth.Application.Interfaces;
using Auth.Domain.Common.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Extensions;

public static class InfrastructureServiceRegistrations
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        var isDevelopment = environment.IsDevelopment();

        services.AddHttpContextAccessor();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordHelper, PasswordHelper>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        services.AddScoped<IScopedLogger, ScopedLogger>();
        services.AddScoped(typeof(IExceptionService),
            isDevelopment
            ? typeof(ExceptionInDevelopmentService) // ExceptionInDevelopmentService
            : typeof(ExceptionInProductionService));

        return services;
    }
}
