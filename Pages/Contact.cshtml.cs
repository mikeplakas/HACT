using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HACT.Models;
using HACT.Services;
using System.Linq;
using System.Threading.Tasks;

namespace HACT.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ContactModel : PageModel
    {
        private readonly EmailSender _emailSender;

        public ContactModel(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [BindProperty]
        public ContactMessage Message { get; set; } = new ContactMessage();

        public void OnGet() { }

        // AJAX handler
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostSendAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var message = System.Text.Json.JsonSerializer.Deserialize<ContactMessage>(body);

            if (message == null)
                return BadRequest(new { success = false, errors = new[] { "Invalid request body." } });

            //TryValidateModel(message);
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage)
            //        .ToList();
            //    return new JsonResult(new { success = false, errors });
            //}

            var toEmail = "info@hact.info";
            var subject = $"Contact Form: {message.Subject}";
            var bodyText = $"Name: {message.Name}\nEmail: {message.Email}\n\n{message.Message}";

            await _emailSender.SendEmailAsync(toEmail, subject, bodyText, message.Email);

            return new JsonResult(new { success = true });
        }

    }
}
