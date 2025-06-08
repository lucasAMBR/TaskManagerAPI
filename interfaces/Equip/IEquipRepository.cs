using Models;

namespace Interfaces{
    public interface IEquipRepository{
        Task<List<Equip>> GetAllAsync();
        Task<Equip> GetByIdAsync(string id);
        Task<List<Equip>> GetAllEquipsByProjectId(string projectId);
        Task<List<Equip>> GetAllEquipsByDevId(string devId);
        Task<Equip> AddAsync(Equip equip);
        Task<Equip> UpdateAsync(Equip equip);
        Task<bool> DeleteAsync(Equip equip); 
    }
}