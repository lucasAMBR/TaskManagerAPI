using DTOs;
using Interfaces;
using Models;

namespace Services{
    public class EquipService : IEquipService {

        private readonly IEquipRepository _equipRepository;

        public EquipService(IEquipRepository equipRepository){
            _equipRepository = equipRepository;
        } 

        public async Task<List<Equip>> GetAllEquipsAsync(){
            return await _equipRepository.GetAllAsync();
        }

        public async Task<Equip> GetEquipByIdAsync(string id){
            return await _equipRepository.GetByIdAsync(id);
        }

        public async Task<Equip> CreateEquipAsync(string managerId, CreateEquipDTO equip){
            Equip newEquip = new Equip
            {
                LeaderId = equip.LeaderId,
                ProjectId = equip.ProjectId,
                Departament = equip.Departament,
                Description = equip.Description
            };

            newEquip.Id = $"TEAM-{DateTime.Now.ToString("yyyyMMddHHmmssff")}";
            
            return await _equipRepository.AddAsync(newEquip);
        }

        public async Task<Equip> UpdateEquipAsync(Equip equip){
            return await _equipRepository.UpdateAsync(equip);
        }

        public async Task<bool> DeleteEquipAsync(string id){
            return await _equipRepository.DeleteAsync(id);
        }
        
    }
}