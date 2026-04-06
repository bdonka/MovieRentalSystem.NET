namespace MovieRentalSystem.NET.Application.Models.Requests.MoviePhysicalCopies;

public class CreateMoviePhysicalCopyRequest
{
    public int MovieId { get; set; }
    public string Code { get; set; } = null!;
}
