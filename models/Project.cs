using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [MaxLength]
        public string? Goals { get; set; }

        public List<Equip> Equips { get; set; } = new();
    }
}