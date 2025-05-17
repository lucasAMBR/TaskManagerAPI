using DTOs;
using Models;

namespace Interfaces{
    public interface IManagerService{
        Task<List<Manager>> GetAllManagersAsync();
        Task<Manager> GetManagerByIdAsync(string id);
        Task<bool> VerifyEmail(string email);
        Task<Manager> CreateManagerAsync(CreateManagerDTO manager);
        Task<Manager> UpdateManagerAsync(string managerID, UpdateManagerDTO manager);
        Task<bool> DeleteManagerAsync(string id);
    }
}