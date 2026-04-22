using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;

public class UpdateMoviePhysicalCopyRequest
{
    public MovieCopyStatus Status { get; set; }
}
