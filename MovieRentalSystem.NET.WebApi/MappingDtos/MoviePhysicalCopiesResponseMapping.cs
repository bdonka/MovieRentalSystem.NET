using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.MappingDtos;

public static class MoviePhysicalCopiesResponseMapping
{
    public static MoviePhysicalCopyResponse MapToMoviePhysicalCopyResponse(this MoviePhysicalCopyDto copy)
    {
        return new MoviePhysicalCopyResponse
        {
            Id = copy.Id,
            MovieId = copy.MovieId,
            Code = copy.Code,
            Status = copy.Status.ToString(),
            Movie = new MovieResponse
            {
                Id = copy.Movie.Id,
                Title = copy.Movie.Title,
                Description = copy.Movie.Description,
                ReleaseYear = copy.Movie.ReleaseYear,
                RentalPrice = copy.Movie.RentalPrice
            }
        };
    }
}
