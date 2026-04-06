namespace MovieRentalSystem.NET.Application.Models.Requests.Movies;

public class UpdateMovieRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public decimal RentalPrice { get; set; }
}
