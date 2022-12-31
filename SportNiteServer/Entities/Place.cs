using NetTopologySuite.Geometries;
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace SportNiteServer.Entities;

public class Place
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string Sport { get; set; } = "";
    public Point Location { get; set; } = new(0, 0);
    public Guid AuthorId { get; set; }
}