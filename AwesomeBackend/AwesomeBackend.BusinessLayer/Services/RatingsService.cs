using AutoMapper;
using AwesomeBackend.DataAccessLayer;
using AwesomeBackend.Shared.Models.Requests;
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
    public class RatingsService : IRatingsService
    {
        private readonly IApplicationDbContext dataContext;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public RatingsService(IApplicationDbContext dataContext, ILogger<RestaurantsService> logger,
            IMapper mapper)
            => (this.dataContext, this.logger, this.mapper) = (dataContext, logger, mapper);

        public async Task<ListResult<Rating>> GetAsync(Guid restaurantId, int pageIndex, int itemsPerPage)
        {
            logger.LogDebug("Trying to retrieve {ItemsCount} ratings for restaurants with Id {RestaurantId}...", itemsPerPage, restaurantId);

            var query = dataContext.GetData<Entities.Rating>().Where(r => r.RestaurantId == restaurantId);
            var totalCount = await query.LongCountAsync();

            logger.LogDebug("Found {ItemsCounts} ratings", totalCount);

            var data = await query
                .OrderByDescending(s => s.Date)
                .Skip(pageIndex * itemsPerPage).Take(itemsPerPage + 1)      // Try to retrieve an element more than the requested number to check whether there are more data.                
                .ToListAsync();

            var ratings = mapper.Map<IEnumerable<Rating>>(data);
            return new ListResult<Rating>(ratings.Take(itemsPerPage), totalCount, ratings.Count() > itemsPerPage);
        }

        public async Task<NewRating> RateAsync(Guid restaurantId, RatingRequest rating)
        {
            // Saves the new rating to the database.
            var dbRating = mapper.Map<Entities.Rating>(rating);
            dbRating.RestaurantId = restaurantId;

            dataContext.Insert(dbRating);
            await dataContext.SaveAsync();

            // Retrieves the new average rating for the restaurant.
            var averageScore = await dataContext.GetData<Entities.Rating>().Where(r => r.RestaurantId == restaurantId).AverageAsync(r => r.Score);
            var result = new NewRating(restaurantId, Math.Round(averageScore, 2));
            return result;
        }
    }
}
