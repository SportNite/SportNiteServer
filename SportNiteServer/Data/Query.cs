using System.Security.Claims;
using SportNiteServer.Entities;
using SportNiteServer.Services;

namespace SportNiteServer.Data;

// Available GraphQL queries
// ReSharper disable once ClassNeverInstantiated.Global
public class Query
{
    public string Version() => "1.0.0";

    [GraphQLDescription("Signed in user")]
    public async Task<User> Me(ClaimsPrincipal claimsPrincipal, AuthService authService)
    {
        return await authService.GetUser(claimsPrincipal);
    }

    [GraphQLDescription("Get offers created by signed user")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Offer>> MyOffers(ClaimsPrincipal claimsPrincipal, AuthService authService,
        OfferService offerService)
    {
        return await offerService.GetMyOffers(await authService.GetUser(claimsPrincipal));
    }

    [GraphQLDescription("Get offers created by signed user")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Response>> GetMyResponses(ClaimsPrincipal claimsPrincipal,
        ResponseService responseService, AuthService authService)
    {
        return await responseService.GetMyResponses(
            await authService.GetUser(claimsPrincipal));
    }

    [GraphQLDescription("Get forecast for specific day and location")]
    public async Task<List<Weather>?> GetForecast(DateTime startDay, double latitude, double longitude)
    {
        return await WeatherService.GetForecast(startDay, latitude, longitude);
    }


    [GraphQLDescription("Get users available on the platform")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<User?> GetUsers(AuthService authService)
    {
        return authService.GetUsers();
    }


    [GraphQLDescription("Get all offers")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Offer?>> GetOffers(OfferService offerService)
    {
        return await offerService.GetOffers();
    }

    [GraphQLDescription("Get all places")]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Place> GetPlaces(PlaceService placeService)
    {
        return placeService.GetPlaces();
    }

    [GraphQLDescription("Get all offers that are accepted and related to signed user")]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<Offer>> IncomingOffers(ClaimsPrincipal claimsPrincipal, AuthService authService,
        OfferService offerService)
    {
        return await offerService.GetIncomingOffers(await authService.GetUser(claimsPrincipal));
    }
}