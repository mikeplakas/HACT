using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using HACT.Models;

namespace HACT.Pages
{
    public class NewsModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<New> News { get; set; } = new();
        public string? CurrentCategory { get; set; }

        public NewsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(string? category)
        {
            ViewData["CshtmlName"] = "News";
            CurrentCategory = category;
            ViewData["SelectedCategory"] = category;
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM News ORDER BY DatePosted DESC";

                if (!string.IsNullOrEmpty(category))
                {
                    query = "SELECT * FROM News WHERE Category = @Category ORDER BY DatePosted DESC";
                }

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(category))
                        cmd.Parameters.AddWithValue("@Category", category);

                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        News.Add(new New
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString() ?? "",
                            Content = reader["Content"].ToString() ?? "",
                            ImageUrl = reader["ImageUrl"].ToString() ?? "/landingpage/img/blog-default.jpg",
                            Category = reader["Category"].ToString() ?? "",
                            DatePosted = (DateTime)reader["DatePosted"]
                        });
                    }
                }
            }
        }
    }
}
