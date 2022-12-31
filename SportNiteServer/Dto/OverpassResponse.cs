namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class OverpassResponse
{
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<OverpassPlace> Elements { get; set; } = new();
}