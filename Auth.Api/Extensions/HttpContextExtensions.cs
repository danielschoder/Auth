namespace Auth.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetJwtToken(this HttpContext context)
        {
            var authorizationHeader = context.Request.Headers.Authorization.ToString();
            return authorizationHeader["Bearer ".Length..].Trim();
        }
    }
}
