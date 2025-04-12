using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories{
    public class DevRepository : IDevRepository {

        private readonly AppDbContext _context;

        public DevRepository(AppDbContext context){
            _context = context;
        }

        public async Task<List<Dev>> GetAllAsync(){
            return await _context.Devs.ToListAsync();
        }

        public async Task<Dev> GetByIdAsync(string id){
            return await _context.Devs.FindAsync(id) ?? throw new Exception($"User {id} not found!!");
        }

        public async Task<Dev> AddAsync(Dev dev){
            _context.Devs.Add(dev);
            await _context.SaveChangesAsync();
            return dev;
        }

        public async Task<Dev> UpdateAsync(Dev dev){
            _context.Devs.Update(dev);
            await _context.SaveChangesAsync();
            return dev;
        }

        public async Task<bool> DeleteAsync(string id){
            var dev = await _context.Devs.FindAsync(id);
            if(dev == null) return false;

            _context.Devs.Remove(dev);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}