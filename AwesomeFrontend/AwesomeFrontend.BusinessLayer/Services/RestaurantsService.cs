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

        public async Task<Restaurant> GetAsync(Guid id)
        {
            try
            {
                var response = await httpClient.GetAsync($"restaurants/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var restaurant = await response.Content.ReadFromJsonAsync<Restaurant>();
                    return restaurant;
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
