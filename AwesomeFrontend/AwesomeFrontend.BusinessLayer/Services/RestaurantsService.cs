using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace AwesomeFrontend.BusinessLayer.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly HttpClient httpClient;

        public RestaurantsService(HttpClient httpClient)
            => this.httpClient = httpClient;

        public async Task<ListResult<Restaurant>> GetAsync(string searchText)
        {
            try
            {
                var response = await httpClient.GetAsync($"restaurants?q={HttpUtility.UrlEncode(searchText)}");
                if (response.IsSuccessStatusCode)
                {
                    var restaurants = await response.Content.ReadFromJsonAsync<ListResult<Restaurant>>();
                    return restaurants;
                }
            }
            catch
            {
            }

            return null;
        }

        public async Task<(Restaurant Restaurant, ListResult<Rating> Ratings)> GetAsync(Guid id)
        {
            try
            {
                var restaurantResponse = await httpClient.GetAsync($"restaurants/{id}");
                var ratingsResponse = await httpClient.GetAsync($"restaurants/{id}/ratings");

                if (restaurantResponse.IsSuccessStatusCode && ratingsResponse.IsSuccessStatusCode)
                {
                    var restaurant = await restaurantResponse.Content.ReadFromJsonAsync<Restaurant>();
                    var ratings = await ratingsResponse.Content.ReadFromJsonAsync<ListResult<Rating>>();
                    return (restaurant, ratings);
                }
            }
            catch
            {
            }

            return (null, null);
        }

        public async Task<(bool Success, ProblemDetails Error)> RateAsync(Guid id, RatingRequest request)
        {
            var response = await httpClient.PostAsJsonAsync($"restaurants/{id}/ratings", request);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            return (false, problemDetails);
        }
    }
}
