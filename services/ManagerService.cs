using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<bool> VerifyEmail(string email)
        {
            return await _managerRepository.VerifyEmailAsync(email);
        }

        public async Task<Manager> CreateManagerAsync(CreateManagerDTO manager)
        {

            var newManager = new Manager
            {
                Id = $"MNG-{manager.Name.Substring(0, 2)}@{DateTime.Now.ToString("yyyyMMddHHmmssff")}",
                Name = manager.Name,
                Email = manager.Email,
                Password = manager.Password
            };

            newManager.Password = PasswordHelper.HashPassword(newManager, manager.Password);

            return await _managerRepository.AddAsync(newManager);
        }

        public async Task<Manager> UpdateManagerAsync(string managerId, UpdateManagerDTO manager){
            var foundedManager = await _managerRepository.GetByIdAsync(managerId);

            if (manager.Name != null)
            {
                foundedManager.Name = manager.Name;
            }

            if (manager.Password != null)
            {
                foundedManager.Password = PasswordHelper.HashPassword(foundedManager, manager.Password);
            }

            return await _managerRepository.UpdateAsync(foundedManager);
        }

        public async Task<bool> DeleteManagerAsync(string id){
            return await _managerRepository.DeleteAsync(id);
        }
    }
}