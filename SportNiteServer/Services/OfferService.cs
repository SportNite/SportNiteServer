using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;

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
        return offer;
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
        return _databaseContext.Offers.Where(x => x.UserId == user.UserId).Include(x => x.Responses);
    }
}