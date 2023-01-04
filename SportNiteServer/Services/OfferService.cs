using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;

namespace SportNiteServer.Services;

public class OfferService
{
    private readonly DatabaseContext _databaseContext;
    private readonly PlaceService _placeService;

    public OfferService(DatabaseContext databaseContext, PlaceService placeService)
    {
        _databaseContext = databaseContext;
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
            City = input.City,
            Street = input.Street,
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
        var responses = await _databaseContext.Responses.Where(x => x.OfferId == id).ToListAsync();
        _databaseContext.RemoveRange(responses);
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

    // Fetch weather for certain offer
    private static async Task<Offer> InjectWeather(Offer offer)
    {
        var weather = await WeatherService.GetWeatherForOffer(offer);
        if (weather != null) offer.Weather = weather;
        return offer;
    }

    // Fetch place for certain offer
    private Offer InjectPlace(Offer offer)
    {
        if (offer.PlaceId != 0)
            offer.Place = _placeService.FindPlace(offer.PlaceId);
        return offer;
    }

    public async Task<IEnumerable<Offer>> GetIncomingOffers(User user)
    {
        var responsesIds =
            (await _databaseContext.Responses
                .Where(x => x.UserId == user.UserId && x.Status == Response.ResponseStatus.Approved).ToListAsync())
            .Select(x => x.OfferId);
        return await _databaseContext.Offers.Where(x =>
                (x.UserId == user.UserId || responsesIds.Contains(x.OfferId)) && x.DateTime > DateTime.Now &&
                !x.IsAvailable)
            .Include(x => x.Responses)
            .ThenInclude(x => x.User)
            .Select(InjectPlace)
            .Select(x =>
            {
                x.Responses = x.Responses.Where(y => y.Status == Response.ResponseStatus.Approved).ToList();
                return x;
            })
            .SelectAsync(InjectWeather);
    }
}