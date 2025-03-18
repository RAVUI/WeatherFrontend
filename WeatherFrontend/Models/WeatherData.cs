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

        public bool IsFavorite { get; set; }
    }
        public class ForecastData
        {
        public string? City { get; set; }

        public bool IsFavorite { get; set; }
        public List<DailyForecast> DailyForecasts { get; set; } = new List<DailyForecast>(); 
        }

        public class DailyForecast
        {
            public DateTime Date { get; set; }
            public string Temperature { get; set; }
            public string Weather { get; set; }
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