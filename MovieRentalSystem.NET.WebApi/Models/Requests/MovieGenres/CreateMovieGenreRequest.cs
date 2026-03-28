using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;

public class CreateMovieGenreRequest
{
    public int MovieId { get; set; }
    public int GenreId { get; set; }
}
