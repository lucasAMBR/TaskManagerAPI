using Interfaces;
using Models;

namespace Services{
    public class ManagerService : IManagerService{
        
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository){
            _managerRepository = managerRepository;
        }

        public async Task<List<Manager>> GetAllManagersAsync(){
            return await _managerRepository.GetAllAsync();
        }

        public async Task<Manager> GetManagerByIdAsync(int id){
            return await _managerRepository.GetByIdAsync(id);
        }

        public async Task<Manager> CreateManagerAsync(Manager manager){
            return await _managerRepository.AddAsync(manager);
        }

        public async Task<Manager> UpdateManagerAsync(Manager manager){
            return await _managerRepository.UpdateAsync(manager);
        }

        public async Task<bool> DeleteManagerAsync(int id){
            return await _managerRepository.DeleteAsync(id);
        }
    }
}