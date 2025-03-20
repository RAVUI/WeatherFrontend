namespace WeatherFrontend.Models
{
    public class LoginResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class AuthCheckResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class MessageResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}