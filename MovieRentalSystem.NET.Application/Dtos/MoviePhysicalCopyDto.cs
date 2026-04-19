using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Dtos;

public class MoviePhysicalCopyDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Code { get; set; } = null!;
    public MovieCopyStatus Status { get; set; } = MovieCopyStatus.Available;
}
