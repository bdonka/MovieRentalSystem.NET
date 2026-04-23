using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class GenreMapping
{
    public static GenreDto MapToGenreDto(this Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public static Genre MapToGenreEntity(this GenreDto genre)
    {
        return new Genre
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
}
