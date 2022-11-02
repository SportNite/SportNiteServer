namespace SportNiteServer.Entities;

public class Weather
{
    public DateTime DateTime { get; set; }
    public double Temperature { get; set; }
    public double WindSpeed { get; set; }
    public double Precipitation { get; set; }
}