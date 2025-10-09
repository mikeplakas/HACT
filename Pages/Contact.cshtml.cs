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
        private readonly IContactService _contactService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ContactModel(IContactService contactService, IHubContext<NotificationHub> hubContext)
        {
            _contactService = contactService;
            _hubContext = hubContext;
        }

        public bool Success { get; set; } = false;

        public void OnGet()
        {
            this.SetupViewDataTitleFromUrl();
        }

        public async Task<IActionResult> OnPost()
        {
            OnGet();

            var name = Request.Form["Name"].ToString();
            var email = Request.Form["Email"].ToString();
            var subject = Request.Form["Subject"].ToString();
            var message = Request.Form["Message"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                return Page();

            var contactMessage = new ContactMessage
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message
            };

            // ? Αποθήκευση στη βάση
            _contactService.Submit(contactMessage);

            // ? Πάρε το πλήθος των unread
            var unreadCount = _contactService.GetUnreadCount();

            // ? Στείλε ειδοποίηση στους Admins
            await _hubContext.Clients.Group("Admins")
                .SendAsync("ReceiveMessage",
                    $"?? Νέο μήνυμα από {contactMessage.Name} ({contactMessage.Email})",
                    unreadCount);

            Success = true;
            return Page();
        }
    }
}
