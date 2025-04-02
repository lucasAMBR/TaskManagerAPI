using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Equip
    {
        [Key]
        public int Id { get; set; }

        public List<Dev> Members { get; set; } = new();

        [ForeignKey("Leader")]
        public int LeaderId { get; set; }
        public required Dev Leader { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public required Project Project { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Departament { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public List<Task> Tasks { get; set; } = new();
    }
}