using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Models;
using HACT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace HACT.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddNewModel : PageModel
    {
        private readonly INewRepository _repository;
        private readonly IWebHostEnvironment _env;

        public AddNewModel(INewRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        [BindProperty]
        public New New { get; set; } = new();

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        [TempData]
        public bool ShowSuccessMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

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

            New.DatePosted = DateTime.UtcNow;
            await _repository.AddNewAsync(New);

            ShowSuccessMessage = true;
            return RedirectToPage();
        }
    }
}
