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
        [MaxLength(20)]
        public required string Priority { get; set; }

        [Required]
        public DateTime InitialDate { get; set; }

        [ForeignKey("Equip")]
        public string EquipId { get; set; } = string.Empty;

        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; } = string.Empty;
        public Dev? Assignee { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Status { get; set; }
    }
}
