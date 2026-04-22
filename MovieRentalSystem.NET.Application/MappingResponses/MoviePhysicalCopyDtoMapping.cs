using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class MoviePhysicalCopyDtoMapping
{
    public static MoviePhysicalCopyDto MapToMoviePhysicalCopyDto(this MoviePhysicalCopy copy)
    {
        return new MoviePhysicalCopyDto
        {
            Id = copy.Id,
            MovieId = copy.MovieId,
            Code = copy.Code
        };
    }
}