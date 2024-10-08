using System.Reflection;
using Auth.BLL;
using Auth.DTO;

namespace Auth.ApiEndpoints;

public static class ApiEndpoints
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/", ServiceAlive);
        app.MapGet("/api", ServiceAlive);

        app.MapPost("/api/login", async (LoginDto loginDto, IUserManager userManager) =>
        {
            try
            {
                var userDto = await userManager.LoginAsync(loginDto);
                return Results.Ok(new LoginResponse { Jwt = userDto.Jwt });
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { ex.Message });
            }
        });

        app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            })
            .UseHttpsRedirection();
        ;
        return app;
    }

    private static ServiceAlive ServiceAlive()
        => new() { Version = Assembly.GetEntryAssembly().GetName().Version.ToString() };
}
