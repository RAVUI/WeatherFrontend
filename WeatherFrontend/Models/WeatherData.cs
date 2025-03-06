namespace WeatherFrontend.Models
{
    public class WeatherData
    {
        public string? City { get; set; }
        public string? Temperature { get; set; }
        public float WindSpeed { get; set; }
        public int Humidity { get; set; }
        public string? Condition { get; set; }
        public string? Weather { get; set; }


    }

    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}