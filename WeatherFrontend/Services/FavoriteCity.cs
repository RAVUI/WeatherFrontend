using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WeatherFrontend.Models;

namespace WeatherFrontend.Services
{
    public interface IFavoriteCityService
    {
        Task<List<FavoriteCity>> GetUserFavoriteCitiesAsync(string userId);
        Task<FavoriteCity> AddToFavoritesAsync(string cityName, string userId);
        Task RemoveFromFavoritesAsync(string cityId, string userId);
        Task<bool> IsCityInFavoritesAsync(string cityName, string userId);
    }

    public class FavoriteCityService : IFavoriteCityService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FavoriteCityService> _logger;

        public FavoriteCityService(HttpClient httpClient, ILogger<FavoriteCityService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<FavoriteCity>> GetUserFavoriteCitiesAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("GetUserFavoriteCitiesAsync called with empty userId");
                    return new List<FavoriteCity>();
                }

                var response = await _httpClient.GetAsync($"v1.0/favoritecities/{userId}"); 

                if (response.IsSuccessStatusCode)
                {
                    var cities = await response.Content.ReadFromJsonAsync<List<FavoriteCity>>();
                    return cities ?? new List<FavoriteCity>();
                }

                _logger.LogError($"Failed to get favorite cities. Status: {response.StatusCode}");
                return new List<FavoriteCity>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving favorite cities for user {userId}");
                return new List<FavoriteCity>();
            }
        }

        public async Task<FavoriteCity> AddToFavoritesAsync(string cityName, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(cityName) || string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentException("City name and user ID are required");
                }

                var favoriteCity = new FavoriteCity
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    CityName = cityName,
                    AddedDate = DateTime.UtcNow
                };

                var response = await _httpClient.PostAsJsonAsync("v1.0/favoritecities", favoriteCity); 

                if (response.IsSuccessStatusCode)
                {
                    var addedCity = await response.Content.ReadFromJsonAsync<FavoriteCity>();
                    return addedCity;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    throw new InvalidOperationException("This city is already in your favorites");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to add city to favorites. Status: {response.StatusCode}, Error: {errorContent}");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding {cityName} to favorites for user {userId}");
                throw;
            }
        }

        public async Task RemoveFromFavoritesAsync(string cityId, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(cityId) || string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentException("City ID and user ID are required");
                }

                var response = await _httpClient.DeleteAsync($"v1.0/favoritecities/{cityId}?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException("City not found or does not belong to the user");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to remove city from favorites. Status: {response.StatusCode}, Error: {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing city {cityId} from favorites for user {userId}");
                throw;
            }
        }

        public async Task<bool> IsCityInFavoritesAsync(string cityName, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(cityName) || string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                var favorites = await GetUserFavoriteCitiesAsync(userId);
                return favorites.Exists(c => c.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if {cityName} is in favorites for user {userId}");
                return false;
            }
        }
    }
}