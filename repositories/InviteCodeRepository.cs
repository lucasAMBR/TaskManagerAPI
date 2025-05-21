using System.Threading.Tasks;
using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public class InviteCodeRepository : IInviteCodeRepository
    {
        private readonly AppDbContext _context;

        public InviteCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InviteCode?> GetInviteByCode(string code)
        {
            return await _context.InviteCodes
                .Include(i => i.Equip)
                .ThenInclude(e => e.Members)
                .FirstOrDefaultAsync(i => i.Code == code);
        } 

        public async Task<string> AddNewCodeAsync(InviteCode invite)
        {
            _context.InviteCodes.Add(invite);
            await _context.SaveChangesAsync();

            return invite.Code;
        }

        public async Task<bool> UpdateCodeAsync(string code, string devId)
        {
            InviteCode? invite = await GetInviteByCode(code);

            if (invite == null || invite.IsExpired || invite.CurrentUsages >= invite.MaxUsages || invite.Equip.Members.Any(d => d.Id == devId))
            {
                return false;
            }


            invite.CurrentUsages++;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}