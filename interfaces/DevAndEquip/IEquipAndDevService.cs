namespace Interfaces{
    public interface IEquipAndDevService
    {
        Task<bool> AddDevToEquip(string equipId, string devId);
        Task<bool> RemoveDevFromEquip(string equipId, string devId);
    }
}