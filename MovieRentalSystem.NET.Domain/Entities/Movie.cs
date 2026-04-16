namespace MovieRentalSystem.NET.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public decimal RentalPrice { get; set; }
        public List<Genre> Genres { get; set; } = [];
        public List<MoviePhysicalCopy> PhysicalCopies { get; set; } = [];
    }
}
