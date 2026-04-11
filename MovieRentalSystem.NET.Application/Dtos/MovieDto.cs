namespace MovieRentalSystem.NET.Application.Dtos;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public decimal RentalPrice { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
    public List<MoviePhysicalCopyDto> PhysicalCopies { get; set; } = new();
}
