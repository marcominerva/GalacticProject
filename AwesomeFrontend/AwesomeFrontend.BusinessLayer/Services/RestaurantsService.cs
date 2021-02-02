using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace AwesomeFrontend.BusinessLayer.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

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
                    var json = await response.Content.ReadAsStringAsync();
                    var restaurants = JsonSerializer.Deserialize<ListResult<Restaurant>>(json, jsonSerializerOptions);

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
                    var json = await restaurantResponse.Content.ReadAsStringAsync();
                    var restaurant = JsonSerializer.Deserialize<Restaurant>(json, jsonSerializerOptions);

                    json = await ratingsResponse.Content.ReadAsStringAsync();
                    var ratings = JsonSerializer.Deserialize<ListResult<Rating>>(json, jsonSerializerOptions);

                    return (restaurant, ratings);
                }
            }
            catch
            {
            }

            return (null, null);
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> RateAsync(Guid id, RatingRequest request)
        {
            var json = JsonSerializer.Serialize(request, jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await httpClient.PostAsync($"restaurants/{id}/ratings", content);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            json = await response.Content.ReadAsStringAsync();
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(json, jsonSerializerOptions);
            var errors = problemDetails.Errors.SelectMany(e => e.Value);

            return (false, errors);
        }
    }
}
