using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;
using SportNiteServer.Exceptions;
using Path = System.IO.Path;

namespace SportNiteServer.Services;

public class PlaceService
{
    private List<Place> _places = new();
    private readonly DatabaseContext _databaseContext;

    public PlaceService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    // Import places from static file (overpass-turbo dump) into SQL database
    public async Task<int> ImportPlaces()
    {
        var content =
            await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(),
                "Assets/sport_objects_krakow.geojson"));
        var overpass = JsonSerializer.Deserialize<OverpassResponse>(content);
        if (overpass != null)
            foreach (var overpassElement in overpass.elements.Where(x => x.type == "node"))
            {
                var place = new Place
                {
                    Id = overpassElement.id,
                    Location = new Point(overpassElement.lon, overpassElement.lat)
                };
                if (overpassElement.tags != null && overpassElement.tags.ContainsKey("name"))
                    place.Name = overpassElement.tags["name"];
                if (overpassElement.tags != null && overpassElement.tags.ContainsKey("sport"))
                    place.Sport = overpassElement.tags["sport"];
                _places.Add(place);
            }

        _places = _places.Where(x => x.Name is {Length: > 0}).ToList();
        Console.WriteLine($"Loaded {_places.Count} places");
        foreach (var place in _places)
            if (!await _databaseContext.Places.AnyAsync(x => x.Id == place.Id))
                _databaseContext.Places.Add(place);
        await _databaseContext.SaveChangesAsync();
        return _places.Count;
    }

    public IEnumerable<Place> GetPlaces()
    {
        return _databaseContext.Places.Where(x => true);
    }

    public Place? FindPlace(long id)
    {
        return id == 0 ? null : _databaseContext.Places.First(x => x.Id == id);
    }


    public async Task<Place> CreatePlace(User user, CreatePlaceInput input)
    {
        if (await _databaseContext.Places.AnyAsync(x => x.Id == input.Id)) throw new DuplicateKeyException();
        var place = new Place
        {
            Id = input.Id,
            Name = input.Name,
            Sport = input.Sport,
            Location = new Point(input.Longitude, input.Latitude),
            AuthorId = user.UserId
        };
        _databaseContext.Places.Add(place);
        await _databaseContext.SaveChangesAsync();
        return place;
    }

    public async Task<Place> DeletePlace(User user, long id)
    {
        var place = _databaseContext.Places.First(x => x.Id == id);
        if (place.AuthorId != user.UserId)
            throw new ForbiddenException();
        _databaseContext.Places.Remove(place);
        await _databaseContext.SaveChangesAsync();
        return place;
    }
}