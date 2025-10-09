using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Data;
using HACT.Models;
using Microsoft.AspNetCore.Hosting;

namespace HACT.Pages.Admin
{
    public class EditNewModel : PageModel
    {
        private readonly INewRepository _NewRepository;
        private readonly IWebHostEnvironment _env;

        public EditNewModel(INewRepository NewRepository, IWebHostEnvironment env)
        {
            _NewRepository = NewRepository;
            _env = env;
        }

        [BindProperty]
        public New New { get; set; } = new();


        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.SetupViewDataTitleFromUrl();
            ViewData["CshtmlName"] = "EditNew";

            New = await _NewRepository.GetByIdAsync(id);
            if (New == null)
                return NotFound();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existingNew = await _NewRepository.GetByIdAsync(New.Id);
            if (existingNew == null)
                return NotFound();

            // Ανέβασμα νέας εικόνας
            if (UploadedImage != null && UploadedImage.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(UploadedImage.FileName);
                var extension = Path.GetExtension(UploadedImage.FileName);
                var newFileName = $"{fileName}-{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_env.WebRootPath, "uploads", newFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(stream);
                }

                New.ImageUrl = "/uploads/" + newFileName;
            }
            else
            {
                New.ImageUrl = existingNew.ImageUrl; // κράτα την παλιά
            }

            New.DatePosted = existingNew.DatePosted; // μην αλλάζει ημερομηνία
            await _NewRepository.UpdateNewAsync(New);

            return RedirectToPage("/News");
        }
    }
}
