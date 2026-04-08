using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.MappingDtos;

public static class GenreResponseMapping
{
    public static GenreResponse MapToGenreResponse(this GenreDto genre)
    {
        return new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
}
