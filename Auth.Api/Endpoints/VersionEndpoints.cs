using Auth.Application.Services;

namespace Auth.Api.Endpoints;

public static class VersionEndpoints
{
    public static void MapVersionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api", (IVersionService versionService) =>
        {
            var version = versionService.GetVersion();
            return Results.Ok(new { version });
        })
        .WithTags("Version")
        .WithName("GetVersion");
    }
}
