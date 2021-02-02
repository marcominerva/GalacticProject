using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using AwesomeFrontend.BusinessLayer.RemoteServices;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AwesomeFrontend.BusinessLayer.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly IAwesomeBackendApi awesomeBackendApi;

        public RestaurantsService(IAwesomeBackendApi awesomeBackendApi)
            => this.awesomeBackendApi = awesomeBackendApi;

        public async Task<ListResult<Restaurant>> GetAsync(string searchText)
        {
            try
            {
                var response = await awesomeBackendApi.GetRestaurantsAsync(searchText);
                if (response.IsSuccessStatusCode)
                {
                    var restaurants = response.Content;
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
                var restaurantResponse = await awesomeBackendApi.GetRestaurantAsync(id);
                var ratingsResponse = await awesomeBackendApi.GetRatingsAsync(id);

                if (restaurantResponse.IsSuccessStatusCode && ratingsResponse.IsSuccessStatusCode)
                {
                    var restaurant = restaurantResponse.Content;
                    var ratings = ratingsResponse.Content;

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
            var response = await awesomeBackendApi.RateAsync(id, request);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            var errors = problemDetails.Errors.SelectMany(e => e.Value);

            return (false, errors);
        }
    }
}
