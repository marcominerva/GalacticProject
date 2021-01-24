using AwesomeBackend.Shared.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AwesomeFrontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;

        [BindProperty(SupportsGet = true, Name = "q")]
        public string SearchText { get; set; }

        public ListResult<Restaurant> Restaurants { get; set; }

        public IndexModel(ILogger<IndexModel> logger) => this.logger = logger;

        public async Task OnGet()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                Restaurants = null;
            }
        }
    }
}
