using DTOs;
using Models;

namespace Interfaces{
    public interface IDevService{
        Task<List<Dev>> GetAllDevsAsync();
        Task<Dev> GetDevByIdAsync(string id);
        Task<Dev> CreateDevAsync(Dev dev);
        Task<Dev> UpdateDevAsync(string devId, UpdateDevDTO dev);
        Task<bool> DeleteDevAsync(string id);
    }
}