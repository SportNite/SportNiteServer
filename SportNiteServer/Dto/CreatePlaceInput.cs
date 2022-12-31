namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class CreatePlaceInput
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Sport { get; set; } = "";

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}