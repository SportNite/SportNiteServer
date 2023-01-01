namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class OverpassPlace
{
    public long id { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
    public string? type { get; set; }
    public Dictionary<string, string>? tags { get; set; }
}