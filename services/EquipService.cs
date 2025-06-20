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

        public async Task<List<Equip>> GetAllEquipsByProjectId(string projectId)
        {
            return await _equipRepository.GetAllEquipsByProjectId(projectId);
        }

        public async Task<List<Equip>> GetAllEquipsByDevId(string devId)
        {
            return await _equipRepository.GetAllEquipsByDevId(devId);
        }

        public async Task<Equip> CreateEquipAsync(string managerId, CreateEquipDTO equip)
        {
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

        public async Task<Equip> UpdateEquipAsync(string managerId, string id, UpdateEquipDTO equip) {
            Equip foundedEquip = await _equipRepository.GetByIdAsync(id);

            if (foundedEquip.Project == null)
            {
                throw new Exception("Error: The specified team was not found.");
            }

            if (foundedEquip.Project.ManagerId != managerId)
            {
                throw new Exception("You can only update teams you manage.");
            }

            if (equip.LeaderId != null){
                foundedEquip.LeaderId = equip.LeaderId;
            }

            if (equip.Departament != null)
            {
                foundedEquip.Departament = equip.Departament;
            }

            if (equip.Description != null)
            {
                foundedEquip.Description = equip.Description;
            }

            return await _equipRepository.UpdateAsync(foundedEquip);
        }

        public async Task<bool> DeleteEquipAsync(string managerId, string equipId){
            var equipWithManager = await _equipRepository.GetByIdAsync(equipId);

            if (equipWithManager.Project == null)
            {
                throw new Exception("Error: The specified team was not found.");
            }

            if (equipWithManager.Project.ManagerId != managerId)
            {
                throw new Exception("Team removal permissions are owner-exclusive.");
            }

            return await _equipRepository.DeleteAsync(equipWithManager);
        }
        
    }
}