using Models;

namespace Interfaces{
    public interface IEquipAndDevRepository
    {
        Task<bool> AddDevToEquipAsync(string equipId, string devId);
        Task<bool> RemoveDevFromEquipAsync(string equipId, string devId);
    }
}