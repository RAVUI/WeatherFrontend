using System.Text.Json.Serialization;

namespace WeatherFrontend.Models
{
    public class UserSearchHistory
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("cityName")]
        public string CityName { get; set; } = string.Empty;

        [JsonPropertyName("temperature")]
        public double  Temperature { get; set; } 

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("windSpeed")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("weatherCondition")]
        public string WeatherCondition { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}