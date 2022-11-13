using System.Security.Claims;
using SportNiteServer.Database;
using SportNiteServer.Entities;
using SportNiteServer.Services;

namespace SportNiteServer.Data;

public class Query
{
    public string version() => "1.0.0";

    public async Task<User> Me(ClaimsPrincipal claimsPrincipal, AuthService authService)
    {
        return await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal));
    }

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Offer>> MyOffers(ClaimsPrincipal claimsPrincipal, AuthService authService,
        OfferService offerService)
    {
        return await offerService.GetMyOffers(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)));
    }

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Response>> GetMyResponses(ClaimsPrincipal claimsPrincipal,
        ResponseService responseService, AuthService authService)
    {
        return await responseService.GetMyResponses(
            await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)));
    }

    public async Task<List<Weather>> GetForecast(DateTime startDay, double latitude, double longitude,
        WeatherService weatherService)
    {
        return await weatherService.GetForecast(startDay, latitude, longitude);
    }

    
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<User?>> GetUsers(AuthService authService)
    {
        return await authService.GetUsers();
    }
}