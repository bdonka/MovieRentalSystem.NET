using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMovieByIdQuery
: IRequest<Result<MovieDto>>
{
    public int Id { get; set; }
}

