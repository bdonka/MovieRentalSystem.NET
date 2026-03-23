using MovieRentalSystem.NET.WebApi.Enums;

namespace MovieRentalSystem.NET.WebApi.Entities
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
