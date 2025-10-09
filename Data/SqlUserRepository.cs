using HACT.Models;
using Microsoft.Data.SqlClient;

namespace HACT.Data
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IConfiguration _config;

        public SqlUserRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            var query = "SELECT TOP 1 * FROM Users WHERE Username = @Username";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = (int)reader["Id"],
                    Username = reader["Username"].ToString() ?? "",
                    PasswordHash = reader["PasswordHash"].ToString() ?? "",
                    IsAdmin = (bool)reader["IsAdmin"]
                };
            }

            return null;
        }
    }
}
