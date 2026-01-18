//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using HACT.Models;
//using HACT.Data;

//namespace HACT.Pages.Admin
//{
//    [Authorize(Roles = "Admin")]
//    public class ContactMessagesModel : PageModel
//    {
//        private readonly IContactMessageRepository _repo;

//        public ContactMessagesModel(IContactMessageRepository repo)
//        {
//            _repo = repo;
//        }

//        public List<ContactMessage> Messages { get; set; } = new();

//        public void OnGet()
//        {
//            Messages = _repo.GetAllMessages();
//        }
//    }
//}
