using SportNiteServer.Entities;

namespace SportNiteServer.Dto;

public class CreateOfferInput
{
    public Guid? OfferId { get; set; }
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Offer.SportType Sport { get; set; }
    public long PlaceId { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
}