using System.ComponentModel.DataAnnotations;

namespace MovieRentalSystem.NET.WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string? Email { get; set; } = null;
        [Required]
        [MinLength(6)]
        public string? Password { get; set; } = null;
        public string? Role { get; set; } = "User";
        public DateTime DateRegistered { get; set; } = DateTime.Now;
    }
}
