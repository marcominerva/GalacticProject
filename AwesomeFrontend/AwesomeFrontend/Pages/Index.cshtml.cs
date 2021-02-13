using AwesomeBackend.Shared.Models.Responses;
using AwesomeFrontend.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AwesomeFrontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IRestaurantsService restaurantsService;

        [BindProperty(SupportsGet = true, Name = "q")]
        public string SearchText { get; set; }

        public ListResult<Restaurant> Restaurants { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IRestaurantsService restaurantsService)
            => (this.logger, this.restaurantsService) = (logger, restaurantsService);

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                Restaurants = await restaurantsService.GetAsync(SearchText);
            }
        }
    }
}
