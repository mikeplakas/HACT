using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Data;
using HACT.Models;

namespace HACT.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewRepository _NewRepo;

        // 3 πιο πρόσφατα άρθρα
        public List<New> LatestArticles { get; set; } = new();

        // 2 πιο πρόσφατα events
        public List<New> LatestEvents { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, INewRepository NewRepo)
        {
            _logger = logger;
            _NewRepo = NewRepo;
        }

        public async Task OnGetAsync()
        {
            if (Request.Query["clear"] == "true")
            {
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            }

            this.SetupViewDataTitleFromUrl();

            var allNews = (await _NewRepo.GetAllAsync()).ToList();

            LatestArticles = allNews
                .Where(x => x.Category == "Articles")
                .OrderByDescending(x => x.DatePosted)
                .Take(6)
                .ToList();

            LatestEvents = allNews
                .Where(x => x.Category == "Events")
                .OrderByDescending(x => x.DatePosted)
                .Take(4)
                .ToList();

        }
    }
}
