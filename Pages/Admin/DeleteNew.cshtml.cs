using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Data;
using HACT.Models;

namespace HACT.Pages.Admin
{
    public class DeleteNewModel : PageModel
    {
        private readonly INewRepository _NewRepository;

        public DeleteNewModel(INewRepository NewRepository)
        {
            _NewRepository = NewRepository;
        }

        [BindProperty]
        public New New { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            New = await _NewRepository.GetByIdAsync(id);
            if (New == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _NewRepository.DeleteNewAsync(id);
            return RedirectToPage("/News");
        }
    }
}
