using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Models;
using HACT.Data;

namespace HACT.Pages
{
    public class NewsDetailModel : PageModel
    {
        private readonly INewRepository _repository;

        public NewsDetailModel(INewRepository repository) => _repository = repository;

        public New? New { get; set; }
        public string? Category { get; set; } // "Articles" | "Events" | null

        public async Task<IActionResult> OnGetAsync(int id, string? category)
        {
            this.SetupViewDataTitleFromUrl();

            // Normalize & validate
            if (!string.IsNullOrWhiteSpace(category))
            {
                var c = category.Trim();
                if (c.Equals("Articles", StringComparison.OrdinalIgnoreCase) ||
                    c.Equals("Events", StringComparison.OrdinalIgnoreCase))
                {
                    Category = char.ToUpper(c[0]) + c.Substring(1).ToLower();
                }
            }

            New = await _repository.GetByIdAsync(id);
            if (New == null) return NotFound();

            ViewData["Title"] = New.Title;
            ViewData["CshtmlName"] = New.Title;
            ViewData["SelectedCategory"] = category; // για navbar, αν θες

            return Page();
        }
    }

}
