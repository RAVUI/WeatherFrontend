namespace WeatherFrontend.Models
{
    public class FavoriteCity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CityName { get; set; }
        public DateTime AddedDate { get; set; }
    }
}

