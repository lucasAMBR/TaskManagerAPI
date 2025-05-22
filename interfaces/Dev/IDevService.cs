using DTOs;
using Models;

namespace Interfaces{
    public interface IDevService{
        Task<List<Dev>> GetAllDevsAsync();
        Task<Dev> GetDevByIdAsync(string id);
        Task<bool> GetByEmailAsync(string email);
        Task<List<Dev>> GetAllMembersByEquipId(string equipId);
        Task<Dev> CreateDevAsync(CreateDevDTO dev);
        Task<Dev> UpdateDevAsync(string devId, UpdateDevDTO dev);
        Task<bool> DeleteDevAsync(string id);
    }
}