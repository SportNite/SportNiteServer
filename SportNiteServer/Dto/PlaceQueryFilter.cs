namespace SportNiteServer.Dto;

public class PlaceQueryFilter
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Radius { get; set; }
    public List<string>? Sports { get; set; }
}