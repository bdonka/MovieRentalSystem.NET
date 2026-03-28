using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Mappings;

public static class MoviePhysicalCopyMapping
{
    public static MoviePhysicalCopyResponse MapToMoviePhysicalCopyResponse(this MoviePhysicalCopy copy)
    {
        return new MoviePhysicalCopyResponse
        {
            Id = copy.Id,
            MovieId = copy.MovieId,
            Code = copy.Code,
            Status = copy.Status
        };
    }
}