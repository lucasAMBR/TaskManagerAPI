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
            return await _context.Equips.FindAsync(id) ?? throw new Exception($"Equip {id} not found!!");
        }

        public async Task<Equip> AddAsync(Equip equip){
            _context.Equips.Add(equip);
            await _context.SaveChangesAsync();
            return equip;
        }

        public async Task<Equip> UpdateAsync(Equip equip){
            _context.Equips.Update(equip);
            await _context.SaveChangesAsync();
            return equip;
        }

        public async Task<bool> DeleteAsync(string id){
            var equip = await _context.Equips.FindAsync(id);
            if(equip == null) return false;

            _context.Equips.Remove(equip);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}