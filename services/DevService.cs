using Interfaces;
using Models;
using Utils;

namespace Services{
    public class DevService : IDevService{

        private readonly IDevRepository _devRepository;

        public DevService(IDevRepository devRespository){
            _devRepository = devRespository;
        } 

        public async Task<List<Dev>> GetAllDevsAsync(){
            return await _devRepository.GetAllAsync();
        }

        public async Task<Dev> GetDevByIdAsync(string id){
            return await _devRepository.GetByIdAsync(id);
        }

        public async Task<Dev> CreateDevAsync(Dev dev){
            dev.Id = $"DEV-{dev.Name.Substring(0, 2)}@{DateTime.Now.ToString("yyyyMMddHHmmssff")}";
            dev.Password = PasswordHelper.HashPassword(dev, dev.Password);

            return await _devRepository.AddAsync(dev);
        }

        public async Task<Dev> UpdateDevAsync(Dev dev){
            return await _devRepository.UpdateAsync(dev);
        }

        public async Task<bool> DeleteDevAsync(string id){
            return await _devRepository.DeleteAsync(id);
        }
    } 
}