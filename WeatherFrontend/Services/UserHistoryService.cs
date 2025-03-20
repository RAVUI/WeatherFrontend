using System.Net.Http.Json;
using System.Text.Json;
using WeatherFrontend.Models;

namespace WeatherFrontend.Services
{
    public class UserHistoryService
    {
        private readonly HttpClient _httpClient;

        public UserHistoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
           
        }

        public async Task<List<UserSearchHistory>> GetUserHistory(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<UserSearchHistory>>($"v1.0/UserHistory/{userId}"); 
        }

        public async Task<UserSearchHistory> AddSearchHistory(UserSearchHistory history)
        {
            var response = await _httpClient.PostAsJsonAsync("v1.0/UserHistory", history); 
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserSearchHistory>();
        }

        public async Task DeleteSearchHistory(string historyId, string userId)
        {
            var response = await _httpClient.DeleteAsync($"v1.0/UserHistory/{historyId}?userId={userId}"); 
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearAllUserSearchHistory(string userId)
        {
            var response = await _httpClient.DeleteAsync($"v1.0/UserHistory/clear/{userId}");
            response.EnsureSuccessStatusCode();
        }
    }
}