namespace DTOs
{
    public class FormattedTaskDTO
    {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public required int Priority { get; set; }
        public required DateTime InitialDate { get; set; }
        public required DateTime FinalDate { get; set; }
        public required string EquipId { get; set; }
        public required string EquipDepartament { get; set; }
        public string? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }
        public required bool IsDone { get; set; }
    }
}