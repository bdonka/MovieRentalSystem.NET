using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class MoviePhysicalCopyMapping
{
    public static MoviePhysicalCopyResponse MapToMoviePhysicalCopyResponse(this MoviePhysicalCopy copy)
    {
        return new MoviePhysicalCopyResponse
        {
            Id = copy.Id,
            MovieId = copy.MovieId,
            Code = copy.Code
        };
    }
}