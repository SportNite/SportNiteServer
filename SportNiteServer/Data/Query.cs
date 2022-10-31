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
    [UseSorting]
    public async Task<IEnumerable<Offer>> MyOffers(ClaimsPrincipal claimsPrincipal, AuthService authService,
        OfferService offerService)
    {
        return await offerService.GetMyOffers(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)));
    }
}