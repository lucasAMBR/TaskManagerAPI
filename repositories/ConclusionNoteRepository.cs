using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public class ConclusionNoteRepository : IConclusionNoteRepository
    {
        private readonly AppDbContext _context;

        public ConclusionNoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConclusionNote> GenerateConclusionNote(ConclusionNote note)
        {
            _context.ConclusionNotes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<List<ConclusionNote>> GetConclusionNotesByEquipId(string equipId) {
                return await _context.ConclusionNotes
                    .Include(cn => cn.Task)
                    .Where(cn => cn.Task != null && cn.Task.EquipId == equipId)
                    .ToListAsync();
        }
    }
}