using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class MovieDtoMapping
{
    public static MovieDto MapToMovieDto(this Movie movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            RentalPrice = movie.RentalPrice,

            Genres = movie.Genres?
            .Select(g => g.MapToGenreDto())
            .ToList() ?? new List<GenreDto>()
        };
    }
}