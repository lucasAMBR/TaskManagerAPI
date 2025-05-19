using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class InviteCode
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = Guid.NewGuid().ToString("N")[..6];

        [Required]
        public required string EquipId { get; set; }

        public required int MaxUsages { get; set; }

        public int CurrentUsages { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(2);

        [NotMapped]
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        [JsonIgnore]
        public Equip? Equip { get; set; }
    }
}