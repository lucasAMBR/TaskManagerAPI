namespace Interfaces{
    public interface IEquipAndDevService{
        Task<bool> AddDevToEquip(string equipId, string devId);
    }
}