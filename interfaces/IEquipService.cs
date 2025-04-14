using Models;

namespace Interfaces{
    public interface IEquipService{
        Task<List<Equip>> GetAllEquipAsync();
        Task<Equip> GetEquipByIdAsync(string id);
        Task<Equip> CreateEquipAsync(Equip equip);
        Task<Equip> UpdateEquipAsync(Equip equip);
        Task<bool> DeleteEquipAsync(string id);
    }
}