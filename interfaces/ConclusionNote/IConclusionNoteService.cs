using DTOs;
using Models;

namespace Interfaces
{
    public interface IConclusionNoteService
    {
        public Task<ConclusionNote> GenerateConclusionNote(string taskId, CreateConclusionNoteDTO note);
    }
}