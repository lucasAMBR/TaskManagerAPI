using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Interfaces;

namespace Models
{
    public class Manager : IUser
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
        
        public Manager(){}
    }
}
