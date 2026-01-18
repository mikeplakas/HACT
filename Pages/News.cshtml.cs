using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using HACT.Models;

namespace HACT.Pages
{
    public class NewsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<New> News { get; set; } = new();

        public NewsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(string? category) => LoadNews(category);

        // ? Επιστρέφει JSON αντί για HTML
        public JsonResult OnGetFilter(string? category)
        {
            LoadNews(category);
            return new JsonResult(News);
        }

        private void LoadNews(string? category)
        {
            News.Clear();
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connStr);
            string query = "SELECT * FROM News";
            if (!string.IsNullOrEmpty(category))
                query += " WHERE Category = @Category";
            query += " ORDER BY DatePosted DESC";

            using var cmd = new SqlCommand(query, conn);
            if (!string.IsNullOrEmpty(category))
                cmd.Parameters.AddWithValue("@Category", category);

            conn.Open();
            using var reader = cmd.ExecuteReader();
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
