
using System.Text.Json.Serialization;

namespace WeatherFrontend.Models
{
    public class WeatherData
    {
        public string? City { get; set; }
        public string? Weather { get; set; }
        public string? Temperature { get; set; }
        public float WindSpeed { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public string? FeelsLike { get; set; }
        public float Visibility { get; set; }
        public int CloudCover { get; set; }

        public bool IsFavorite { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class SimpleForecast
    {
        public string Date { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string Weather { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public float WindSpeed { get; set; }
        public int Pressure { get; set; }
        public string FeelsLike { get; set; } = string.Empty;
        public string Sunrise { get; set; } = string.Empty;   
        public string Sunset { get; set; } = string.Empty;    
    }

    public class ErrorResponse
    {
        public string? Message { get; set; }
    }

    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}