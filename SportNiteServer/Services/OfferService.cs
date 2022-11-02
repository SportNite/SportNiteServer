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

    public OfferService(DatabaseContext databaseContext, AuthService authService)
    {
        _databaseContext = databaseContext;
        _authService = authService;
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
            .SelectAsync(InjectWeather);
    }

    public static async Task<Offer> InjectWeather(Offer offer)
    {
        var response = await new HttpClient().GetAsync(
            "https://api.open-meteo.com/v1/forecast?latitude=" + offer.Latitude + "&longitude=" + offer.Longitude +
            "&hourly=temperature_2m,precipitation,windspeed_10m,rain&start_date=" +
            offer.DateTime.ToString("yyyy-MM-dd") + "&end_date=" + offer.DateTime.ToString("yyyy-MM-dd"));
        var responseString = await response.Content.ReadAsStringAsync();
        dynamic? data = JsonConvert.DeserializeObject(responseString);
        if (data == null) return offer;
        offer.Weather = new Weather
        {
            DateTime = offer.DateTime,
            Temperature = Utils.Average(data.hourly.temperature_2m.ToObject<List<double>>()),
            WindSpeed = Utils.Average(data.hourly.windspeed_10m.ToObject<List<double>>()),
            Precipitation = Utils.Average(data.hourly.precipitation.ToObject<List<double>>()),
        };
        return offer;
    }
}