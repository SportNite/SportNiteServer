using SportNiteServer.Entities;

namespace SportNiteServer.Dto;

public class CreateOfferInput
{
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Offer.SportType Sport { get; set; }
}