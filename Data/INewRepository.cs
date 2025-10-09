using HACT.Models;

namespace HACT.Data
{
    public interface INewRepository
    {
        Task<IEnumerable<New>> GetAllAsync();
        Task<New?> GetByIdAsync(int id);
        Task AddNewAsync(New New);
        Task UpdateNewAsync(New New);
        Task DeleteNewAsync(int id);
        Task<List<New>> GetLatestNewsAsync(int count);
        

    }
}
