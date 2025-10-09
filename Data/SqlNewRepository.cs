using Microsoft.Data.SqlClient;
using HACT.Models;
using Microsoft.Extensions.Configuration;

namespace HACT.Data
{
    public class SqlNewRepository : INewRepository
    {
        private readonly IConfiguration _configuration;

        public SqlNewRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString() => _configuration.GetConnectionString("DefaultConnection")!;

        public async Task<List<New>> GetLatestNewsAsync(int count)
        {
            var News = new List<New>();

            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var query = "SELECT TOP (@Count) * FROM News ORDER BY DatePosted DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Count", count);
                    conn.Open();
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        News.Add(ReadNew(reader));
                    }
                }
            }

            return News;
        }

        public async Task<IEnumerable<New>> GetAllAsync()
        {
            var News = new List<New>();

            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var query = "SELECT * FROM News ORDER BY DatePosted DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        News.Add(ReadNew(reader));
                    }
                }
            }

            return News;
        }


        public async Task<New?> GetByIdAsync(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var query = "SELECT * FROM News WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return ReadNew(reader);
                    }
                }
            }

            return null;
        }

        public async Task AddNewAsync(New New)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var query = @"INSERT INTO News (Title, Content, ImageUrl, Category, DatePosted)
                              VALUES (@Title, @Content, @ImageUrl, @Category, @DatePosted)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", New.Title);
                    //cmd.Parameters.AddWithValue("@Author", New.Author);
                    cmd.Parameters.AddWithValue("@Content", New.Content);
                    cmd.Parameters.AddWithValue("@ImageUrl", New.ImageUrl);
                    cmd.Parameters.AddWithValue("@Category", New.Category);
                    cmd.Parameters.AddWithValue("@DatePosted", New.DatePosted);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateNewAsync(New New)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var query = @"UPDATE News SET 
                              Title = @Title,                           
                              Content = @Content,
                              ImageUrl = @ImageUrl,
                              Category = @Category,
                              DatePosted = @DatePosted
                              WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", New.Id);
                    cmd.Parameters.AddWithValue("@Title", New.Title);
                    //cmd.Parameters.AddWithValue("@Author", New.Author);
                    cmd.Parameters.AddWithValue("@Content", New.Content);
                    cmd.Parameters.AddWithValue("@ImageUrl", New.ImageUrl);
                    cmd.Parameters.AddWithValue("@Category", New.Category);
                    cmd.Parameters.AddWithValue("@DatePosted", New.DatePosted);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteNewAsync(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var query = "DELETE FROM News WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        private New ReadNew(SqlDataReader reader)
        {
            return new New
            {
                Id = (int)reader["Id"],
                Title = reader["Title"].ToString() ?? string.Empty,
                //Author = reader["Author"].ToString() ?? string.Empty,
                Content = reader["Content"].ToString() ?? string.Empty,
                ImageUrl = reader["ImageUrl"].ToString() ?? string.Empty,
                Category = reader["Category"].ToString() ?? string.Empty,
                DatePosted = (DateTime)reader["DatePosted"]
            };
        }
    }
}
