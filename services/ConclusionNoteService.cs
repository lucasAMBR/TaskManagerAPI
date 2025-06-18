using DTOs;
using Interfaces;
using Models;

namespace Services
{
    public class ConclusionNoteService : IConclusionNoteService
    {

        private readonly IConclusionNoteRepository _conclusionNoteRepository;

        public ConclusionNoteService(IConclusionNoteRepository conclusionNoteRepository)
        {
            _conclusionNoteRepository = conclusionNoteRepository;
        }

        public async Task<ConclusionNote> GenerateConclusionNote(string taskId, CreateConclusionNoteDTO note)
        {
            ConclusionNote newNote = new ConclusionNote
            {
                Id = $"CNT-{DateTime.Now.ToString("yyyyMMddHHmmssff")}",
                TaskId = taskId,
                Type = note.Type,
                Notes = note.Note,
                HoursSpend = note.HoursSpend,
            };

            return await _conclusionNoteRepository.GenerateConclusionNote(newNote);
        }

        public async Task<List<ConclusionNote>> ListAllConclusionNotesByEquipId(string equipId)
        {
            return await _conclusionNoteRepository.GetConclusionNotesByEquipId(equipId);
        }
    }
}