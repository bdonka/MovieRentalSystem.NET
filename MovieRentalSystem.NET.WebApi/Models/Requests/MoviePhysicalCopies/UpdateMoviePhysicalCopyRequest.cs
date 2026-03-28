using MovieRentalSystem.NET.WebApi.Enums;

namespace MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;

public class UpdateMoviePhysicalCopyRequest
{
    public MovieCopyStatus Status { get; set; }
}
