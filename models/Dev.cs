using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Dev
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [JsonIgnore]
        public List<Equip> Equips { get; set; } = new();

        [JsonIgnore]
        public List<Task> Tasks { get; set; } = new();

        public Dev(){}
    }
}
