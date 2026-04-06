using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Domain.Entities
{
    public class MoviePhysicalCopy
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Code { get; set; } = null!;
        public MovieCopyStatus Status { get; set; } = MovieCopyStatus.Available;
        public Movie Movie { get; set; } = null!;
    }
}
