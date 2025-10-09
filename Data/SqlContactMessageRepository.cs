using HACT.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace HACT.Data
{
    public class SqlContactMessageRepository : IContactMessageRepository
    {
        private readonly string _connectionString;

        public SqlContactMessageRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void SaveMessage(ContactMessage message)
        {
            using var conn = new SqlConnection(_connectionString);
            string query = @"INSERT INTO ContactMessages (Name, Email, Subject, Message, CreationDate)
                             VALUES (@Name, @Email, @Subject, @Message, @CreationDate)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", message.Name);
            cmd.Parameters.AddWithValue("@Email", message.Email);
            cmd.Parameters.AddWithValue("@Subject", message.Subject);
            cmd.Parameters.AddWithValue("@Message", message.Message);
            cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<ContactMessage> GetAllMessages()
        {
            var messages = new List<ContactMessage>();

            using var conn = new SqlConnection(_connectionString);
            string query = @"SELECT Id, Name, Email, Subject, Message, CreationDate
                             FROM ContactMessages
                             ORDER BY CreationDate DESC";

            using var cmd = new SqlCommand(query, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                messages.Add(new ContactMessage
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Subject = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Message = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    CreationDate = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5)
                });
            }


            return messages;
        }

        public ContactMessage? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            string query = @"SELECT Id, Name, Email, Subject, Message, CreationDate
                             FROM ContactMessages
                             WHERE Id = @Id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new ContactMessage
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Subject = reader.GetString(3),
                    Message = reader.GetString(4),
                    CreationDate = reader.GetDateTime(5)
                };
            }

            return null;
        }

        public List<ContactMessage> GetUnreadMessages()
        {
            var messages = new List<ContactMessage>();
            using var conn = new SqlConnection(_connectionString);
            string query = "SELECT Id, Name, Email, Subject, Message, CreationDate, IsRead FROM ContactMessages WHERE IsRead = 0";
            using var cmd = new SqlCommand(query, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new ContactMessage
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Subject = reader.GetString(3),
                    Message = reader.GetString(4),
                    CreationDate = reader.GetDateTime(5),
                    IsRead = reader.GetBoolean(6)
                });
            }
            return messages;
        }

        public int GetUnreadCount()
        {
            using var conn = new SqlConnection(_connectionString);
            string query = "SELECT COUNT(*) FROM ContactMessages WHERE IsRead = 0";
            using var cmd = new SqlCommand(query, conn);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void MarkAllAsRead(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            string query = "UPDATE ContactMessages SET IsRead = 1 WHERE Id = @Id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
