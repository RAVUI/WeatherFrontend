
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherFrontend.Models;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
    public async Task<WeatherData?> GetCurrentWeatherAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("Invalid city name.");
            return null;
        }

        try
        {
            
            string apiUrl = $"v1.0/weather/search?city={Uri.EscapeDataString(city)}";
            Console.WriteLine($"Calling API: {apiUrl}");

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            Console.WriteLine($"API response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<WeatherData>();
                Console.WriteLine($"Got weather for city: {result?.City ?? "unknown"}");
                return result;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode}, Details: {errorContent}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in GetCurrentWeatherAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<List<SimpleForecast>?> GetWeatherForecastAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("Invalid city name.");
            return null;
        }

        try
        {
            
            string apiUrl = $"v1.0/weather/forecast?city={Uri.EscapeDataString(city)}";
            Console.WriteLine($"Calling API: {apiUrl}");

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            Console.WriteLine($"API response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var simpleForecasts = await response.Content.ReadFromJsonAsync<List<SimpleForecast>>();
                Console.WriteLine($"Got forecast for city: {city}");
                return simpleForecasts;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode}, Details: {errorContent}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in GetWeatherForecastAsync: {ex.Message}");
            return null;
        }
    }

    
    public async Task<WeatherData?> GetWeatherByCoordinatesAsync(double latitude, double longitude)
    {
        try
        {
            Console.WriteLine($"Getting weather for coordinates: {latitude}, {longitude}");

           
            string apiUrl = $"v1.0/weather/current-location?lat={latitude}&lon={longitude}";
            Console.WriteLine($"Calling API: {apiUrl}");

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            Console.WriteLine($"API response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<WeatherData>();
                Console.WriteLine($"Got weather for city: {result?.City ?? "unknown"}");
                return result;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode}, Details: {errorContent}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in GetWeatherByCoordinatesAsync: {ex.Message}");
            return null;
        }
    }

    
    public async Task<List<WeatherData>> GetSurroundingWeatherByGridAsync(double centerLat, double centerLon, double radiusKm = 50, int numPoints = 10)
    {
        var surroundingWeather = new List<WeatherData>();
        var random = new Random();

        try
        {
            
            double degreeOffset = radiusKm / 111.0; 

            
            for (int i = 0; i < numPoints; i++)
            {
                
                double angle = random.NextDouble() * 2 * Math.PI; 
                double distance = random.NextDouble() * degreeOffset; 

                
                double latOffset = distance * Math.Cos(angle);
                double lonOffset = distance * Math.Sin(angle);

                double newLat = centerLat + latOffset;
                double newLon = centerLon + lonOffset;

                
                if (newLat < -90 || newLat > 90 || newLon < -180 || newLon > 180) continue;

                var weather = await GetWeatherByCoordinatesAsync(newLat, newLon);
                if (weather != null)
                {
                    
                    weather.Latitude = newLat;
                    weather.Longitude = newLon;
                    surroundingWeather.Add(weather);
                }
            }

            Console.WriteLine($"Fetched weather for {surroundingWeather.Count} surrounding locations.");
            return surroundingWeather;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in GetSurroundingWeatherByGridAsync: {ex.Message}");
            return surroundingWeather;
        }
    }
}