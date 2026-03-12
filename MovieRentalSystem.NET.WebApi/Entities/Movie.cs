namespace MovieRentalSystem.NET.WebApi.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int ReleaseYear { get; set; }
        public decimal RentalPrice { get; set; }

    }
}
