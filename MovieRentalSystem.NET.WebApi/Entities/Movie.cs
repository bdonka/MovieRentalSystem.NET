namespace MovieRentalSystem.NET.WebApi.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public decimal RentalPrice { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<MoviePhysicalCopy> PhysicalCopies { get; set; } = new List<MoviePhysicalCopy>();
    }
}
