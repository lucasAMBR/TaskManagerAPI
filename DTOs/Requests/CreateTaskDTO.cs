namespace DTOs
{
    public class CreateTaskDTO
    {
        public required string Description { get; set; }
        public required int Priority { get; set; }
        public required DateTime InitialDate { get; set; }
        public required DateTime FinalDate { get; set; }
        public required string EquipId { get; set; }
        public string? AssigneeId { get; set; } 
    }
}