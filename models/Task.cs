using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Task
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        public required int Priority { get; set; }

        [Required]
        public DateTime InitialDate { get; set; }

        [Required]
        public DateTime FinalDate { get; set; }

        [ForeignKey("Equip")]
        public string EquipId { get; set; } = string.Empty;
        [JsonIgnore]
        public Equip? Equip { get; set; }

        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; } = string.Empty;
        [JsonIgnore]
        public Dev? Assignee { get; set; }

        public bool IsDone { get; set; } = false; 

        public Task() { }
    }
}
