using NetTopologySuite.Geometries;

namespace SportNiteServer.Dto;

public class CreatePlaceInput
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Sport { get; set; }
   
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}