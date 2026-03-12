using System.ComponentModel.DataAnnotations;

namespace MovieRentalSystem.NET.WebApi.Entities
{
    public class MoviePhysicalCopy
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Available";
    }
}
