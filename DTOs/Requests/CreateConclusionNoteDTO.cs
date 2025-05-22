using Enums;

namespace DTOs
{
    public class CreateConclusionNoteDTO
    {
        public string Type { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public double HoursSpend { get; set; }
    }
}