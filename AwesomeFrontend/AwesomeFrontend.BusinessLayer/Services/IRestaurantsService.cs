using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using System;
using System.Threading.Tasks;

namespace AwesomeFrontend.BusinessLayer.Services
{
    public interface IRestaurantsService
    {
        Task<ListResult<Restaurant>> GetAsync(string searchText);

        Task<(Restaurant Restaurant, ListResult<Rating> Ratings)> GetAsync(Guid id);

        Task<(bool Success, ProblemDetails Error)> RateAsync(Guid id, RatingRequest request);
    }
}