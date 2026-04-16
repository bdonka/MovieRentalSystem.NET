using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Infrastructure.Mapping;

public static class MoviePhysicalCopyMapping
{
    public static MoviePhysicalCopyDto MapToMoviePhysicalCopyDto(this MoviePhysicalCopy copy)
    {
        return new MoviePhysicalCopyDto
        {
            Id = copy.Id,
            MovieId = copy.MovieId,
            Code = copy.Code,
            Movie = new MovieDto
            {
                Id = copy.Movie.Id,
                Title = copy.Movie.Title,
                Description = copy.Movie.Description,
                ReleaseYear = copy.Movie.ReleaseYear,
                RentalPrice = copy.Movie.RentalPrice
            }
        };
    }

    public static MoviePhysicalCopy MapToMoviePhysicalCopyEntity(this MoviePhysicalCopyDto copy)
    {
        return new MoviePhysicalCopy
        {
            Id = copy.Id,
            MovieId = copy.MovieId,
            Code = copy.Code
        };
    }
}
