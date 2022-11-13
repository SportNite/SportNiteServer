using Newtonsoft.Json;

namespace SportNiteServer.Entities;

public class Place
{
    public long Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Name { get; set; }
    public string Sport { get; set; }
}