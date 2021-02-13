using AutoMapper;
using AwesomeBackend.DataAccessLayer;
using AwesomeBackend.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = AwesomeBackend.DataAccessLayer.Entities;

namespace AwesomeBackend.BusinessLayer.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly IApplicationDbContext dataContext;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public RestaurantsService(IApplicationDbContext dataContext, ILogger<RestaurantsService> logger,
            IMapper mapper)
            => (this.dataContext, this.logger, this.mapper) = (dataContext, logger, mapper);

        public async Task<Restaurant> GetAsync(Guid id)
        {
            var dbRestaurant = await dataContext.GetData<Entities.Restaurant>().Include(r => r.Ratings).FirstOrDefaultAsync(v => v.Id == id);
            if (dbRestaurant == null)
            {
                logger.LogInformation("Unable to find restaurant with Id {RestaturantId}", id);
                return null;
            }

            var restaurant = mapper.Map<Restaurant>(dbRestaurant);
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
                .ToListAsync();

            var restaurants = mapper.Map<IEnumerable<Restaurant>>(data);
            return new ListResult<Restaurant>(restaurants.Take(itemsPerPage), totalCount, restaurants.Count() > itemsPerPage);
        }
    }
}
