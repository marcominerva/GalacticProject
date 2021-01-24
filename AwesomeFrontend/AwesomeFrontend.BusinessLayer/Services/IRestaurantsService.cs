using AwesomeBackend.Shared.Models.Responses;
using System;
using System.Threading.Tasks;

namespace AwesomeFrontend.BusinessLayer.Services
{
    public interface IRestaurantsService
    {
        Task<ListResult<Restaurant>> GetAsync(string searchText);

        Task<Restaurant> GetAsync(Guid id);
    }
}