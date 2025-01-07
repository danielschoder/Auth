using Auth.Contracts.ExternalServices;

namespace Auth.Api.Endpoints;

public static class VisitorsEndpoints
{
    public static void MapVisitorsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/visitors", AddVisitorAsync);

        static void AddVisitorAsync(HttpContext httpContext, ISlackClient slackClient)
        {
            var request = httpContext.Request;
            var clientIp = request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                request.HttpContext.Connection.RemoteIpAddress?.ToString();
            slackClient.SendMessage(clientIp);
        }
    }
}
