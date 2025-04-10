using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Equip
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Dev> Members { get; set; } = new();

        [ForeignKey("Leader")]
        public string LeaderId { get; set; } = string.Empty;

        [JsonIgnore]
        public required Dev Leader { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; } = string.Empty;

        [JsonIgnore]
        public required Project Project { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Departament { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [JsonIgnore]
        public List<Task> Tasks { get; set; } = new();
    }
}
