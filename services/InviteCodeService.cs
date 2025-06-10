using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Services
{
    public class InviteCodeService : IInviteCodeService
    {

        private readonly IEquipRepository _equipRepository;
        private readonly IInviteCodeRepository _inviteCodeRepository;
        private readonly IEquipAndDevRepository _equipAndDevRepository;

        public InviteCodeService(IEquipRepository equipRepository, IInviteCodeRepository inviteCodeRepository, IEquipAndDevRepository equipAndDevRepository)
        {
            _equipRepository = equipRepository;
            _inviteCodeRepository = inviteCodeRepository;
            _equipAndDevRepository = equipAndDevRepository;
        }

        public async Task<string> GenerateInviteCode(string userId, string userRole, string equipId, int maxUsages)
        {
            Equip equip = await _equipRepository.GetByIdAsync(equipId);

            if (userRole == "DEV")
            {
                if (userId != equip.LeaderId)
                {
                    throw new Exception("Only team leaders and project managers can generate invites.");
                }
            }
            if (userRole == "MNG")
            {
                if (equip.Project == null)
                {
                    throw new Exception("You can only generate invites for existing project teams.");
                }
                if (userId != equip.Project.ManagerId)
                {
                    throw new Exception("Invite generation is restricted to team leaders and project managers.");
                }
            }

            InviteCode newInvite = new InviteCode
            {
                EquipId = equipId,
                MaxUsages = maxUsages,
                Equip = equip
            };

            newInvite.Id =  $"INV-{DateTime.Now.ToString("yyyyMMddHHmmssff")}";

            var generatedInvite = await _inviteCodeRepository.AddNewCodeAsync(newInvite);

            return generatedInvite;
        }

        public async Task<bool> UseInviteCode(string userId, string code)
        {
            var invite = await _inviteCodeRepository.UpdateCodeAsync(code, userId);

            if (invite)
            {
                var equipIdfromCode = await _inviteCodeRepository.GetInviteByCode(code) ?? throw new Exception("Can not find this invite");

                return await _equipAndDevRepository.AddDevToEquipAsync(equipIdfromCode.EquipId, userId);
            }

            return false;
        }
    }
}