using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Enums;

namespace Models
{
    public class ConclusionNote
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [ForeignKey("Task")]
        public string TaskId { get; set; } = string.Empty;
        public Models.Task? Task { get; set; }

        public string Type { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;

        public double HoursSpend { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}