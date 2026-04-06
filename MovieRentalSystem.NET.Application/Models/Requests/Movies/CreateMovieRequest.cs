namespace MovieRentalSystem.NET.Application.Models.Requests.Movies;

public class CreateMovieRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public decimal RentalPrice { get; set; }
}
