using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class GenreMapping
{
    public static GenreResponse MapToGenreResponse(this Genre genre)
    {
        return new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

}