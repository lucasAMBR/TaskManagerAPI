using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Priority {get; set;}

        [Required]
        public DateTime InitialDate { get; set; }

        [ForeignKey("Assignee")]
        public int AssigneeId { get; set; }
        public required Dev Assignee { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Status { get; set; }
    }
}