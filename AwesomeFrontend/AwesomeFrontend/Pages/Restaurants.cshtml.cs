using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using AwesomeFrontend.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeFrontend.Pages
{
    public class RestaurantModel : PageModel
    {
        private readonly ILogger<RestaurantModel> logger;
        private readonly IRestaurantsService restaurantsService;

        public Restaurant Restaurant { get; set; }

        public ListResult<Rating> Ratings { get; set; }

        [BindProperty]
        public RatingRequest RatingRequest { get; set; } = new RatingRequest();

        public IEnumerable<SelectListItem> RatingScores { get; set; }

        public RestaurantModel(ILogger<RestaurantModel> logger, IRestaurantsService restaurantsService)
            => (this.logger, this.restaurantsService) = (logger, restaurantsService);

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                await GetPageDataAsync(id);
                return Page();
            }

            return NotFound();
        }

        private async Task GetPageDataAsync(Guid id)
        {
            (Restaurant, Ratings) = await restaurantsService.GetAsync(id);

            RatingScores = new List<SelectListItem>() { new("(select)", "-1") }.Union(
                                Enumerable.Range(1, 5).Select(x =>
                                   new SelectListItem
                                   {
                                       Value = x.ToString(),
                                       Text = x.ToString()
                                   }));
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var rateResult = await restaurantsService.RateAsync(id, RatingRequest);
            if (rateResult.Success)
            {
                return Redirect(Request.Path);
            }

            foreach (var error in rateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            await GetPageDataAsync(id);
            return Page();
        }
    }
}
