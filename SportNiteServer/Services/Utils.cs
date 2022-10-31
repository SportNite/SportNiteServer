using System.Security.Claims;

namespace SportNiteServer.Services;

public class Utils
{
    public static string? GetFirebaseUserId(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(x => x.Type == "user_id")
            ? claimsPrincipal.Claims.First(x => x.Type == "user_id").Value
            : null;
    }
}