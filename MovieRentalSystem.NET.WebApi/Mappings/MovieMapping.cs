using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Mappings;

public static class MovieMapping
{
    public static MovieResponse MapToMovieResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            RentalPrice = movie.RentalPrice
        };
    }
}