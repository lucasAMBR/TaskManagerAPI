using Models;

namespace DTOs
{
    public class TaskResponseDTO
    {
        public required Models.Task Task { get; set; }
        public ConclusionNote? ConclusionNote { get; set; }
    }
}