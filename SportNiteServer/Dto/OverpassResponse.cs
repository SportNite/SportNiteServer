using Newtonsoft.Json;
using SportNiteServer.Entities;

namespace SportNiteServer.Dto;

public class OverpassResponse
{
    public List<OverpassPlace> elements { get; set; }
}