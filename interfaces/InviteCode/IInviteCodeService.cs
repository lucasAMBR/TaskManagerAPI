using Models;

namespace Interfaces
{
    public interface IInviteCodeService
    {
        public Task<string> GenerateInviteCode(string userId, string userRole, string equipId, int maxUsages);

        public Task<bool> UseInviteCode(string devId, string code);
    }
}