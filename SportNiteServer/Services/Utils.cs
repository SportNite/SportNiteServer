using System.Security.Claims;

namespace SportNiteServer.Services;

public static class Utils
{
    // Extract Firebase user id from JWT token claims
    public static string? GetFirebaseUserId(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(x => x.Type == "user_id")
            ? claimsPrincipal.Claims.First(x => x.Type == "user_id").Value
            : null;
    }

    // IEnumerable mapper, but for async transformer
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method)
    {
        return await Task.WhenAll(source.Select(async s => await method(s)));
    }

    // Averages list of doubles
    public static double Average(List<double> items)
    {
        var sum = items.Sum();
        return Math.Round(sum / items.Count, 1);
    }
}