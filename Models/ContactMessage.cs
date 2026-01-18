using System.ComponentModel.DataAnnotations;

namespace HACT.Models
{
    public class ContactMessage
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100, ErrorMessage = "Name must be up to 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        [StringLength(150, ErrorMessage = "Subject must be up to 150 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter your message.")]
        [StringLength(5000, ErrorMessage = "Message must be less than 5000 characters.")]
        public string Message { get; set; }
    }
}
