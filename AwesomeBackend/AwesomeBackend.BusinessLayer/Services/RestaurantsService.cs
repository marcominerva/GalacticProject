using AwesomeBackend.DataAccessLayer;
using AwesomeBackend.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Entities = AwesomeBackend.DataAccessLayer.Entities;

namespace AwesomeBackend.BusinessLayer.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly IApplicationDbContext dataContext;
        private readonly ILogger logger;

        public RestaurantsService(IApplicationDbContext dataContext, ILogger<RestaurantsService> logger)
            => (this.dataContext, this.logger) = (dataContext, logger);

        public async Task<Restaurant> GetAsync(Guid id)
        {
            var dbRestaurant = await dataContext.GetData<Entities.Restaurant>().Include(r => r.Ratings).FirstOrDefaultAsync(v => v.Id == id);
            if (dbRestaurant == null)
            {
                logger.LogInformation("Unable to find restaurant with Id {RestaturantId}", id);
                return null;
            }

            var restaurant = CreateRestaurantDto(dbRestaurant);
            return restaurant;
        }

        public async Task<ListResult<Restaurant>> GetAsync(string searchText, int pageIndex, int itemsPerPage)
        {
            logger.LogDebug("Trying to retrieve a max of {ItemsCount} restaurants using {SearchText} query...", itemsPerPage, searchText);

            var query = dataContext.GetData<Entities.Restaurant>()
                .Where(r => searchText == null || r.Name.Contains(searchText) || r.Address.Location.Contains(searchText));

            var totalCount = await query.LongCountAsync();

            logger.LogDebug("{ItemsCount} restaurants found", totalCount);

            var data = await query.Include(r => r.Ratings)
                .OrderBy(r => r.Name)
                //.OrderByDescending(r => r.Ratings.Select(rating => rating.Score).DefaultIfEmpty(0).Average())
                .Skip(pageIndex * itemsPerPage).Take(itemsPerPage + 1)      // Try to retrieve an element more than the requested number to check whether there are more data.
                .Select(dbRestaurant => CreateRestaurantDto(dbRestaurant))
                .ToListAsync();

            return new ListResult<Restaurant>(data.Take(itemsPerPage), totalCount, data.Count > itemsPerPage);
        }

        private static Restaurant CreateRestaurantDto(Entities.Restaurant dbRestaurant)
        {
            return new Restaurant
            {
                Id = dbRestaurant.Id,
                Name = dbRestaurant.Name,
                Address = new Address
                {
                    Location = dbRestaurant.Address.Location,
                    PostalCode = dbRestaurant.Address.PostalCode,
                    Province = dbRestaurant.Address.Province,
                    Street = dbRestaurant.Address.Street
                },
                Email = dbRestaurant.Email,
                ImageUrl = dbRestaurant.ImageUrl,
                PhoneNumber = dbRestaurant.PhoneNumber,
                WebSite = dbRestaurant.WebSite,
                RatingsCount = dbRestaurant.Ratings?.Count ?? 0,
                RatingScore = dbRestaurant.Ratings?.Count > 0 ? Math.Round(dbRestaurant.Ratings.Select(r => r.Score).Average(), 2) : null
            };
        }
    }
}
