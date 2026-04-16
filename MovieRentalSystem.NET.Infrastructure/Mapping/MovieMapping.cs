using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Infrastructure.Mapping;

public static class MovieMapping
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
            Genres = movie.Genres?.Select(g => g.MapToGenreDto()).ToList() ?? new List<GenreDto>(),
            PhysicalCopies = movie.PhysicalCopies?.Select(g => g.MapToMoviePhysicalCopyDto()).ToList() ?? new List<MoviePhysicalCopyDto>()
        };
    }

    public static Movie MapToMovieEntity(this MovieDto movie)
    {
        return new Movie
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            RentalPrice = movie.RentalPrice
        };
    }
}
