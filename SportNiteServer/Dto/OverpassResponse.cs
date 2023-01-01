namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class OverpassResponse
{
    // ReSharper disable once CollectionNeverUpdated.Global
    // Lower case name because JSON key serialization
    // ReSharper disable once InconsistentNaming
    public List<OverpassPlace> elements { get; set; } = new();
}