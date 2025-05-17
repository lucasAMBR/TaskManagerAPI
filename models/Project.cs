using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [MaxLength]
        public string? Goals { get; set; }

        [ForeignKey("Manager")]
        public string ManagerId { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Equip> Equips { get; set; } = new();

        public Project() { }
    }
}
