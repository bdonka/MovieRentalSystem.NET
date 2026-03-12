using System.ComponentModel.DataAnnotations;

namespace MovieRentalSystem.NET.WebApi.Entities
{
    public class MovieGenre
    {
        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}
