using Models;

namespace Interfaces{
    public interface IDevService{
        Task<List<Dev>> GetAllDevsAsync();
        Task<Dev> GetDevByIdAsync(string id);
        Task<Dev> CreateDevAsync(Dev dev);
        Task<Dev> UpdateDevAsync(Dev dev);
        Task<bool> DeleteDevAsync(string id);
    }
}