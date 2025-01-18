using System.Security.Claims;

namespace Auth.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
            => Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

        public static string GetIdAsLogString(this ClaimsPrincipal user)
        {
            return TryGetId(out var userId) ? $" [userId: {userId}]" : string.Empty;

            bool TryGetId(out Guid userId)
            {
                userId = Guid.Empty;

                if (user is null) { return false; };
                if (!user?.Identity?.IsAuthenticated == true) { return false; };

                return Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            }
        }
    }
}
