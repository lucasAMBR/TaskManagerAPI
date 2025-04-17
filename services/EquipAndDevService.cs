using Interfaces;

namespace Services{
    public class EquipAndDevService : IEquipAndDevService{
        
        private readonly IEquipAndDevRepository _equipAndDevRepository;

        public EquipAndDevService(IEquipAndDevRepository equipAndDevRepository){
            _equipAndDevRepository = equipAndDevRepository;
        }

        public async Task<bool> AddDevToEquip(string equipId, string devId){
            return await _equipAndDevRepository.AddDevToEquipAsync(equipId, devId);
        }
    }

}