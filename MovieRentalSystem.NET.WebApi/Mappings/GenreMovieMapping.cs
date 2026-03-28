using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Mappings;

public static class MovieGenreMapping
{
    public static MovieGenreResponse MapToMovieGenreResponse(this MovieGenre movieGenre)
    {
        return new MovieGenreResponse
        {
            MovieId = movieGenre.MovieId,
            GenreId = movieGenre.GenreId
        };
    }
}