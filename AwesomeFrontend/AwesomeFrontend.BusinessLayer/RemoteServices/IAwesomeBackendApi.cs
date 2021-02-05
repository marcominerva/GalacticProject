using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AwesomeFrontend.BusinessLayer.RemoteServices
{
    public interface IAwesomeBackendApi
    {
        [Get("/restaurants")]
        Task<ApiResponse<ListResult<Restaurant>>> GetRestaurantsAsync([AliasAs("q")] string searchText);

        [Get("/restaurants/{id}")]
        Task<ApiResponse<Restaurant>> GetRestaurantAsync(Guid id);

        [Get("/restaurants/{id}/ratings")]
        Task<ApiResponse<ListResult<Rating>>> GetRatingsAsync(Guid id);

        [Post("/restaurants/{id}/ratings")]
        Task<HttpResponseMessage> RateAsync(Guid id, RatingRequest request);
    }
}
