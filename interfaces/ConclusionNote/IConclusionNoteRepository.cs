using Models;

namespace Interfaces
{
    public interface IConclusionNoteRepository
    {
        public Task<ConclusionNote> GenerateConclusionNote(ConclusionNote note);
    }
}