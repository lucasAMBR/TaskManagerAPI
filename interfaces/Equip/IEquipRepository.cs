using Models;

namespace Interfaces{
    public interface IEquipRepository{
        Task<List<Equip>> GetAllAsync();
        Task<Equip> GetByIdAsync(string id);
        Task<Equip> AddAsync(Equip equip);
        Task<Equip> UpdateAsync(Equip equip);
        Task<bool> DeleteAsync(string id); 
    }
}