using System.Security.Claims;

namespace SportNiteServer.Services;

public static class Utils
{
    public static string? GetFirebaseUserId(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(x => x.Type == "user_id")
            ? claimsPrincipal.Claims.First(x => x.Type == "user_id").Value
            : null;
    }

    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method)
    {
        return await Task.WhenAll(source.Select(async s => await method(s)));
    }

    public static double Average(List<double> items)
    {
        var sum = items.Sum();
        return Math.Round(sum / items.Count, 1);
    }
}