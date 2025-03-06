using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WeatherFrontend.Models;


namespace WeatherFrontend.Service
{


    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigation;

        public AuthService(HttpClient httpClient, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _navigation = navigation;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _httpClient.PostAsync($"api/auth/login?email={email}&password={password}", null);
            if (response.IsSuccessStatusCode)
            {
                // Cookies are set by the backend; no need to handle tokens here
                return true;
            }
            return false;
        }

        public async Task<bool> Register(string email, string password)
        {
            var response = await _httpClient.PostAsync($"api/auth/register?email={email}&password={password}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync("api/auth/logout", null);
            _navigation.NavigateTo("/login", forceLoad: true);
        }

        public async Task<bool> IsAuthenticated()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/auth/check");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // In AuthService.cs
        public async Task EnsureTokenIsValid()
        {
            var response = await _httpClient.GetAsync("api/auth/check");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var refreshResponse = await _httpClient.PostAsync("api/auth/refresh", null);
                if (refreshResponse.IsSuccessStatusCode)
                {
                    return;
                }
                await Logout(); // If refresh fails, log out
            }
        }


    }

}