using DTOs;
using Models;

namespace Interfaces{
    public interface IEquipService{
        Task<List<Equip>> GetAllEquipsAsync();
        Task<Equip> GetEquipByIdAsync(string id);
        Task<Equip> CreateEquipAsync(string managerId, CreateEquipDTO equip);
        Task<List<Equip>> GetAllEquipsByProjectId(string projectId);
        Task<List<Equip>> GetAllEquipsByDevId(string devId);
        Task<Equip> UpdateEquipAsync(string managerID, string id, UpdateEquipDTO equip);
        Task<bool> DeleteEquipAsync(string managerId, string equipId);
    }
}