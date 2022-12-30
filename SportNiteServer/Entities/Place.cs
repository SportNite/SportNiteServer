using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace SportNiteServer.Entities;

public class Place
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string Sport { get; set; }
    public Point Location { get; set; }
    public Guid AuthorId { get; set; }
}