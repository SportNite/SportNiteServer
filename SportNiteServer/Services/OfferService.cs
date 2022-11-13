using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;
using System.Linq;

namespace SportNiteServer.Services;

public class OfferService
{
    private readonly DatabaseContext _databaseContext;
    private readonly AuthService _authService;
    private readonly WeatherService _weatherService;
    private readonly PlaceService _placeService;

    public OfferService(DatabaseContext databaseContext, AuthService authService, WeatherService weatherService,
        PlaceService placeService)
    {
        _databaseContext = databaseContext;
        _authService = authService;
        _weatherService = weatherService;
        _placeService = placeService;
    }

    public async Task<Offer> CreateOffer(User user, CreateOfferInput input)
    {
        var offer = new Offer
        {
            UserId = user.UserId,
            Description = input.Description ?? "",
            DateTime = input.DateTime,
            Latitude = input.Latitude,
            Longitude = input.Longitude,
            Sport = input.Sport,
            IsAvailable = true,
            PlaceId = input.PlaceId
        };
        if (input.OfferId != null) offer.OfferId = input.OfferId.Value;
        await _databaseContext.Offers.AddAsync(offer);
        await _databaseContext.SaveChangesAsync();
        return await InjectWeather(offer);
    }

    public async Task<Offer> DeleteOffer(User user, Guid id)
    {
        var offer = await _databaseContext.Offers.Where(x => x.UserId == user.UserId && x.OfferId == id).FirstAsync();
        _databaseContext.Offers.Remove(offer);
        await _databaseContext.SaveChangesAsync();
        return offer;
    }

    public async Task<IEnumerable<Offer>> GetMyOffers(User user)
    {
        return await _databaseContext.Offers.Where(x => x.UserId == user.UserId)
            .Include(x => x.Responses)
            .ThenInclude(x => x.User)
            .Select(InjectPlace)
            .SelectAsync(InjectWeather);
    }

    public async Task<IEnumerable<Offer>> GetOffers()
    {
        return await _databaseContext.Offers
            .Include(x => x.Responses)
            .ThenInclude(x => x.User)
            .Select(InjectPlace)
            .SelectAsync(InjectWeather);
    }

    private async Task<Offer> InjectWeather(Offer offer)
    {
        var weather = await _weatherService.GetWeatherForOffer(offer);
        if (weather != null) offer.Weather = weather;
        return offer;
    }

    private Offer InjectPlace(Offer offer)
    {
        offer.Place = _placeService.FindPlace(offer.PlaceId);
        return offer;
    }
}