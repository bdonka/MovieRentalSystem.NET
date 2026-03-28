namespace MovieRentalSystem.NET.WebApi.Models.Responses;

public class MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public decimal RentalPrice { get; set; }
}
