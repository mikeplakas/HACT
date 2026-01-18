using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Models;
using HACT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HACT.Hubs; // για το NotificationHub

namespace HACT.Pages
{
    public class ContactModel : PageModel
    {
        private readonly EmailSender _emailSender;

        public ContactModel(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [BindProperty]
        public ContactMessage Message { get; set; }

        public bool Success { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Εδώ βάζετε το email που θέλετε να λαμβάνει τα μηνύματα
            var toEmail = "info@hact.info";
            var subject = $"Contact Form: {Message.Subject}";
            var body = $"Name: {Message.Name}\nEmail: {Message.Email}\n\n{Message.Message}";

            await _emailSender.SendEmailAsync(toEmail, subject, body, Message.Email);

            Success = true;
            ModelState.Clear();
            return Page();
        }
    }
}
