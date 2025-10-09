using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HACT.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty] public string Username { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByNameAsync(Username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index"); // Þ "/Admin/ContactMessages"
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid credentials");
            return Page();
        }
    }
}
