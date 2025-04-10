using Models;

namespace Interfaces{
    public interface IManagerService{
        Task<List<Manager>> GetAllManagersAsync();
        Task<Manager> GetManagerByIdAsync(int id);
        Task<Manager> CreateManagerAsync(Manager manager);
        Task<Manager> UpdateManagerAsync(Manager manager);
        Task<bool> DeleteManagerAsync(int id);
    }
}