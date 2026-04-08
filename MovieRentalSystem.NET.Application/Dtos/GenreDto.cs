namespace MovieRentalSystem.NET.Application.Dtos;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<MovieDto> Movies { get; } = [];
}
