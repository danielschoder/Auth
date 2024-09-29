using Auth.Application.Services;

namespace Auth.Api.Endpoints;

public static class VersionEndpoints
{
    public static void MapVersionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", ServiceAlive);
        app.MapGet("/api", ServiceAlive)
            .WithTags("Version")
            .WithName("GetVersion");

        static string ServiceAlive(IVersionService versionService)
            => versionService.GetVersion();
    }
}
