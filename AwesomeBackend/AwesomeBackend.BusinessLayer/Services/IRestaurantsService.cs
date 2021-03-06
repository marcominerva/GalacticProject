﻿using AwesomeBackend.Shared.Models.Responses;
using System;
using System.Threading.Tasks;

namespace AwesomeBackend.BusinessLayer.Services
{
    public interface IRestaurantsService
    {
        Task<Restaurant> GetAsync(Guid id);

        Task<ListResult<Restaurant>> GetAsync(string searchText, int pageIndex, int itemsPerPage);
    }
}