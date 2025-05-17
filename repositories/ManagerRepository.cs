using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories{
    public class ManagerRepository : IManagerRepository{

        private readonly AppDbContext _context;

        public ManagerRepository(AppDbContext context){
            _context = context;
        }

        public async Task<List<Manager>> GetAllAsync(){
            return await _context.Managers.ToListAsync();
        }

        public async Task<Manager> GetByIdAsync(string id){
            var manager = await _context.Managers.FindAsync(id) ?? throw new Exception($"UserID {id} not found!!");

            return manager;
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            return await _context.Managers.AnyAsync(u => u.Email == email);
        }

        public async Task<Manager> AddAsync(Manager manager)
        {
            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<Manager> UpdateAsync(Manager manager){
            _context.Managers.Update(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<bool> DeleteAsync(string id){
            var manager = await _context.Managers.FindAsync(id);
            if(manager == null) return false;

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}