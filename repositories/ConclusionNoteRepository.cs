using Data;
using Interfaces;
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
    }
}