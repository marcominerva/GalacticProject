using AwesomeBackend.Shared.Models.Responses;
using AwesomeFrontend.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AwesomeFrontend.Pages
{
    public class RestaurantModel : PageModel
    {
        private readonly ILogger<RestaurantModel> logger;
        private readonly IRestaurantsService restaurantsService;

        [BindProperty(SupportsGet = true, Name = "q")]
        public string SearchText { get; set; }

        public Restaurant Restaurant { get; set; }

        public RestaurantModel(ILogger<RestaurantModel> logger, IRestaurantsService restaurantsService)
            => (this.logger, this.restaurantsService) = (logger, restaurantsService);

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                Restaurant = await restaurantsService.GetAsync(id);
                return Page();
            }

            return NotFound();
        }
    }
}
