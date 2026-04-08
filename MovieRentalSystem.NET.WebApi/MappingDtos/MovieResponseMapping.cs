using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.MappingDtos;

public static class MovieResponseMapping
{
    public static MovieResponse MapToMovieResponse(this MovieDto movie)
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
