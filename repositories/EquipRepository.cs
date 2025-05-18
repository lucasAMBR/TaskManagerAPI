using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories{
    public class EquipRepository : IEquipRepository{

        private readonly AppDbContext _context;

        public EquipRepository(AppDbContext context){
            _context = context;
        }

        public async Task<List<Equip>> GetAllAsync(){
            return await _context.Equips.ToListAsync();
        }

        
        public async Task<Equip> GetByIdAsync(string id){
            return await _context.Equips.Include(e => e.Project).FirstOrDefaultAsync(e => e.Id == id) ?? throw new Exception($"Equip {id} not found!!");
        }

        public async Task<Equip> AddAsync(Equip equip){
            equip.Leader = await _context.Devs.FindAsync(equip.LeaderId);
            equip.Project = await _context.Projects.FindAsync(equip.ProjectId);

            _context.Equips.Add(equip);
            await _context.SaveChangesAsync();
            return equip;
        }

        public async Task<Equip> UpdateAsync(Equip equip){ 
            equip.Leader = await _context.Devs.FindAsync(equip.LeaderId);

            _context.Equips.Update(equip);
            await _context.SaveChangesAsync();
            return equip;
        }

        public async Task<bool> DeleteAsync(Equip equip){
            _context.Equips.Remove(equip);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}