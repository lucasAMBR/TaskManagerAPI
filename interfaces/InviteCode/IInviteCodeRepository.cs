using Models;

namespace Interfaces
{
    public interface IInviteCodeRepository
    {
        public Task<InviteCode?> GetInviteByCode(string code);
        public Task<string> AddNewCodeAsync(InviteCode code);
        public Task<bool> UpdateCodeAsync(string code, string devId);   
    }
}