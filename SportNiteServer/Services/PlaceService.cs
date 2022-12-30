using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;
using Path = System.IO.Path;

namespace SportNiteServer.Services;

public class PlaceService
{
    private List<Place> _places = new();

    private readonly DatabaseContext _databaseContext;

    public PlaceService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        var content =
            File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Assets/sport_objects_krakow.geojson"));
        var overpass = JsonSerializer.Deserialize<OverpassResponse>(content);
        foreach (var overpassElement in overpass.elements.Where(x => x.type == "node"))
        {
            var place = new Place
            {
                Id = overpassElement.id,
                Latitude = overpassElement.lat,
                Longitude = overpassElement.lon,
            };
            if (overpassElement.tags != null && overpassElement.tags.ContainsKey("name"))
                place.Name = overpassElement.tags["name"];
            if (overpassElement.tags != null && overpassElement.tags.ContainsKey("sport"))
                place.Sport = overpassElement.tags["sport"];
            _places.Add(place);
        }

        _places = _places.Where(x => x.Name is {Length: > 0}).ToList();
        Console.WriteLine($"Loaded {_places.Count} places");

        ImportPlaces();
    }

    public async Task ImportPlaces()
    {
        foreach (var place in _places)
            if (!await _databaseContext.Places.AnyAsync(x => x.Id == place.Id))
                _databaseContext.Places.Add(place);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Place>> GetPlaces()
    {
        // return _places.Where(
        //         x => DistanceTo(x.Latitude, x.Longitude, queryFilter.Latitude, queryFilter.Longitude) < queryFilter.Radius)
        //     .Where(x => ((queryFilter.Sports ?? new List<string>()).Count <= 0 ||
        //                  (queryFilter.Sports ?? new List<string>()).Contains(x.Sport)))
        //     .ToList();
        return _databaseContext.Places.Where(x => true);
    }

    public Place FindPlace(long id)
    {
        return _places.First(x => x.Id == id);
    }

    public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
    {
        var rlat1 = Math.PI * lat1 / 180;
        var rlat2 = Math.PI * lat2 / 180;
        var theta = lon1 - lon2;
        var rtheta = Math.PI * theta / 180;
        var dist =
            Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;
        return unit switch
        {
            'K' => //Kilometers -> default
                dist * 1.609344,
            'N' => //Nautical Miles 
                dist * 0.8684,
            'M' => //Miles
                dist,
            _ => dist
        };
    }
}