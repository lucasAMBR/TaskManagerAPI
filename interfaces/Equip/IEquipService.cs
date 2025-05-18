using DTOs;
using Models;

namespace Interfaces{
    public interface IEquipService{
        Task<List<Equip>> GetAllEquipsAsync();
        Task<Equip> GetEquipByIdAsync(string id);
        Task<Equip> CreateEquipAsync(string managerId, CreateEquipDTO equip);
        Task<Equip> UpdateEquipAsync(string managerID, string id, UpdateEquipDTO equip);
        Task<bool> DeleteEquipAsync(string id);
    }
}