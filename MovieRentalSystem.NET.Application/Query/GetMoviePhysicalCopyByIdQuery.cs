using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyByIdQuery
: IRequest<Result<MoviePhysicalCopyDto>>
{
    public int Id { get; set; }
}

