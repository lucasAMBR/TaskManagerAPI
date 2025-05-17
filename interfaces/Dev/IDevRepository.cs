using Models;

namespace Interfaces{
    public interface IDevRepository{
        Task<List<Dev>> GetAllAsync();
        Task<Dev> GetByIdAsync(string id);
        Task<Dev> AddAsync(Dev dev);
        Task<Dev> UpdateAsync(Dev dev);
        Task<bool> DeleteAsync(string id);
    }
}