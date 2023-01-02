using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportNiteServer.Entities;

namespace SportNiteServer.Services;

public class WeatherService
{
    public static async Task<Weather?> GetWeatherForOffer(Offer offer)
    {
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(8);
            var response = await client.GetAsync(
                "https://api.open-meteo.com/v1/forecast?latitude=" + offer.Latitude + "&longitude=" + offer.Longitude +
                "&hourly=temperature_2m,precipitation,windspeed_10m,rain&start_date=" +
                offer.DateTime.ToString("yyyy-MM-dd") + "&end_date=" + offer.DateTime.ToString("yyyy-MM-dd"));
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic? data = JsonConvert.DeserializeObject(responseString);
            if (data == null) return null;
            return new Weather
            {
                DateTime = offer.DateTime,
                Temperature = Utils.Average(data.hourly.temperature_2m.ToObject<List<double>>()),
                WindSpeed = Utils.Average(data.hourly.windspeed_10m.ToObject<List<double>>()),
                Precipitation = Utils.Average(data.hourly.precipitation.ToObject<List<double>>())
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<List<Weather>?> GetForecast(DateTime startDay, double latitude, double longitude)
    {
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(8);
            var response = await client.GetAsync("https://api.open-meteo.com/v1/forecast?latitude=" +
                                                 latitude + "&longitude=" + longitude +
                                                 "&hourly=temperature_2m,precipitation,windspeed_10m,rain&start_date=" +
                                                 startDay.ToString("yyyy-MM-dd") + "&end_date=" +
                                                 startDay.AddDays(7).ToString("yyyy-MM-dd"));
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic? data = JsonConvert.DeserializeObject(responseString);
            if (data == null) return null;
            var items = new List<Weather>();
            var i = 0;
            foreach (JValue time in data.hourly.time)
            {
                if (time.Value != null)
                    items.Add(new Weather
                    {
                        DateTime = DateTime.Parse(time.Value.ToString() ?? string.Empty),
                        Temperature = data.hourly.temperature_2m[i],
                        Precipitation = data.hourly.precipitation[i],
                        WindSpeed = data.hourly.windspeed_10m[i]
                    });
                i++;
            }

            return items;
        }
        catch (Exception)
        {
            return new List<Weather>();
        }
    }
}