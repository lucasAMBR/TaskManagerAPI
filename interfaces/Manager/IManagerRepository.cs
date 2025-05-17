using Models;

namespace Interfaces{
    public interface IManagerRepository{
        Task<List<Manager>> GetAllAsync();
        Task<Manager> GetByIdAsync(string id);
        Task<bool> VerifyEmailAsync(string email);
        Task<Manager> AddAsync(Manager manager);
        Task<Manager> UpdateAsync(Manager manager);
        Task<bool> DeleteAsync(string id);
    }
}