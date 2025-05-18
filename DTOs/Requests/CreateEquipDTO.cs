namespace DTOs
{
    public class CreateEquipDTO
    {
        public required string LeaderId { get; set; }
        public required string ProjectId { get; set; }
        public required string Departament { get; set; }
        public required string Description { get; set; }
    }
}