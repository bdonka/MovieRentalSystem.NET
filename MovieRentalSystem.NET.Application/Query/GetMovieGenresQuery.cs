using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMovieGenresQuery
: IRequest<Result<IEnumerable<GenreDto>>>
{
    public int MovieId { get; set; }
}

