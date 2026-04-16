using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.WebApi.Models.Responses;

public class MoviePhysicalCopyResponse
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Code { get; set; } = null!;
    public string Status { get; set; } = null!;
    public MovieResponse? Movie { get; set; }
}
