using Models;

namespace Interfaces{
    public interface IEquipAndDevRepository{
        Task<bool> AddDevToEquipAsync(string equipId, string devId);
    }
}