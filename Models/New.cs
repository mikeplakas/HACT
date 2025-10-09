using System.ComponentModel.DataAnnotations;

namespace HACT.Models
{
    public class New
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        //public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty; // "News" ή "Events"
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
    }
}