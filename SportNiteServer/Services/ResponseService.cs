using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;

namespace SportNiteServer.Services;

public class ResponseService
{
    private readonly DatabaseContext _databaseContext;
    private readonly WeatherService _weatherService;

    public ResponseService(DatabaseContext databaseContext, WeatherService weatherService)
    {
        _databaseContext = databaseContext;
        _weatherService = weatherService;
    }

    public async Task<Response?> CreateResponse(User user, CreateResponseInput input)
    {
        var responses = await _databaseContext.Responses.Where(x => x.OfferId == input.OfferId).ToListAsync();
        if (responses.Any(x => x.Status == Response.ResponseStatus.Approved)) return null;
        if (responses.Any(x => x.UserId == user.UserId && x.Status == Response.ResponseStatus.Pending)) return null;
        var response = new Response()
        {
            OfferId = input.OfferId,
            Description = input.Description,
            UserId = user.UserId,
            Status = Response.ResponseStatus.Pending,
        };
        if (input.ResponseId != null) response.ResponseId = input.ResponseId.Value;
        await _databaseContext.Responses.AddAsync(response);
        await _databaseContext.SaveChangesAsync();
        return response;
    }

    public async Task<IEnumerable<Response>> GetMyResponses(User user)
    {
        return await _databaseContext.Responses.Where(x => x.UserId == user.UserId)
            .Include(x => x.Offer)
            .Include(x => x.User)
            .SelectAsync(async x =>
            {
                var weather = await _weatherService.GetWeatherForOffer(x.Offer);
                if (weather != null) x.Offer.Weather = weather;
                return x;
            });
    }

    public async Task<Response> DeleteResponse(User user, Guid id)
    {
        var response = await _databaseContext.Responses.Where(x => x.UserId == user.UserId && x.ResponseId == id)
            .FirstAsync();
        _databaseContext.Responses.Remove(response);
        await _databaseContext.SaveChangesAsync();
        return response;
    }

    public async Task<Response?> RejectResponse(User user, Guid id)
    {
        var response = await _databaseContext.Responses
            .Where(x => x.ResponseId == id)
            .Include(x => x.Offer)
            .Include(x => x.User)
            .FirstAsync();
        if (response.Offer.UserId != user.UserId)
            return null;
        if (response.Status != Response.ResponseStatus.Pending)
            return null;
        response.Status = Response.ResponseStatus.Rejected;
        await _databaseContext.SaveChangesAsync();
        return response;
    }

    public async Task<Response?> AcceptResponse(User user, Guid id)
    {
        var response = await _databaseContext.Responses
            .Where(x => x.ResponseId == id)
            .Include(x => x.Offer)
            .Include(x => x.User)
            .FirstAsync();
        if (response.Offer.UserId != user.UserId)
            return null;
        if (response.Status != Response.ResponseStatus.Pending)
            return null;
        response.Status = Response.ResponseStatus.Approved;
        await _databaseContext.SaveChangesAsync();
        var responses = await _databaseContext.Responses.Where(x => x.OfferId == response.OfferId).ToListAsync();
        foreach (var item in responses.Where(item => item.ResponseId != response.ResponseId))
            item.Status = Response.ResponseStatus.Canceled;
        await _databaseContext.SaveChangesAsync();

        var offer = await _databaseContext.Offers.Where(x => x.OfferId == response.OfferId)
            .FirstAsync();
        offer.IsAvailable = false;
        _databaseContext.Update(offer);
        await _databaseContext.SaveChangesAsync();

        return response;
    }
}