using Interfaces;
using Models;
using Utils;

namespace Services{
    public class ManagerService : IManagerService{
        
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository){
            _managerRepository = managerRepository;
        }

        public async Task<List<Manager>> GetAllManagersAsync(){
            return await _managerRepository.GetAllAsync();
        }

        public async Task<Manager> GetManagerByIdAsync(string id){
            return await _managerRepository.GetByIdAsync(id);
        }

        public async Task<Manager> CreateManagerAsync(Manager manager){
            manager.Id = $"MNG-{manager.Name.Substring(0, 2)}@{DateTime.Now.ToString("yyyyMMddHHmmssff")}";
            manager.Password = PasswordHelper.HashPassword(manager, manager.Password);
            
            return await _managerRepository.AddAsync(manager);
        }

        public async Task<Manager> UpdateManagerAsync(Manager manager){
            return await _managerRepository.UpdateAsync(manager);
        }

        public async Task<bool> DeleteManagerAsync(string id){
            return await _managerRepository.DeleteAsync(id);
        }
    }
}