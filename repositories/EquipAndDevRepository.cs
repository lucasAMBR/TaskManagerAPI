using Data;
using Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories{
    public class EquipAndDevRepository : IEquipAndDevRepository
    {

        private readonly AppDbContext _context;

        public EquipAndDevRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDevToEquipAsync(string equipId, string devId)
        {

            Equip? equip = await _context.Equips.Include(e => e.Members).FirstOrDefaultAsync(e => e.Id == equipId);
            Dev? dev = await _context.Devs.FirstOrDefaultAsync(d => d.Id == devId);

            if (equip == null || dev == null)
            {
                return false;
            }

            equip.Members.Add(dev);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveDevFromEquipAsync(string equipId, string devId)
        {
            var equip = await _context.Equips.Include(e => e.Members).FirstOrDefaultAsync(e => e.Id == equipId);
            var dev = await _context.Devs.FirstOrDefaultAsync(d => d.Id == devId);

            if (equip == null || dev == null)
            {
                return false;
            }

            if (!equip.Members.Contains(dev))
            {
                return false;
            }

            equip.Members.Remove(dev);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}