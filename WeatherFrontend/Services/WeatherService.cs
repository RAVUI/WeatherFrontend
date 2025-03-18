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

    // Fetch current weather by city name
    public async Task<WeatherData?> GetCurrentWeatherAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("Invalid city name.");
            return null;
        }

        try
        {
            string apiUrl = $"api/weather/search?city={Uri.EscapeDataString(city)}";
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
        string apiUrl = $"api/weather/forecast?city={Uri.EscapeDataString(city)}";
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

    // Fetch weather by coordinates (Latitude & Longitude)
    public async Task<WeatherData?> GetWeatherByCoordinatesAsync(double latitude, double longitude)
    {
        try
        {
            Console.WriteLine($"Getting weather for coordinates: {latitude}, {longitude}");

            string apiUrl = $"api/weather/current-location?lat={latitude}&lon={longitude}";
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
}