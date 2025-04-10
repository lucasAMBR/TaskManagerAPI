using Models;

namespace Interfaces{
    public interface IManagerRepository{
        Task<List<Manager>> GetAllAsync();
        Task<Manager> GetByIdAsync(int id);
        Task<Manager> AddAsync(Manager manager);
        Task<Manager> UpdateAsync(Manager manager);
        Task<bool> DeleteAsync(int id);
    }
}