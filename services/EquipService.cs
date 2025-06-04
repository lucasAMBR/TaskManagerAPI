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

        public async Task<Equip> UpdateEquipAsync(string managerId, string id, UpdateEquipDTO equip) {
            Equip foundedEquip = await _equipRepository.GetByIdAsync(id);

            if (foundedEquip.Project == null)
            {
                throw new Exception("You can not update a equip that does not exist!");
            }

            if (foundedEquip.Project.ManagerId != managerId)
            {
                throw new Exception("You can not update someone else's equip");
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
                throw new Exception("You can not delete a equip that does not exist!");
            }

            if (equipWithManager.Project.ManagerId != managerId)
            {
                throw new Exception("You can not delete someone else's equip");
            }

            return await _equipRepository.DeleteAsync(equipWithManager);
        }
        
    }
}