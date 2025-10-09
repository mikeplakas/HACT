using HACT.Models;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}
