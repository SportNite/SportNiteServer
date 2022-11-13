namespace SportNiteServer.Dto;

public class OverpassPlace
{
    public long id { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
    public string? name { get; set; }
    public string? type { get; set; }
    public Dictionary<string, string>? tags { get; set; }
}