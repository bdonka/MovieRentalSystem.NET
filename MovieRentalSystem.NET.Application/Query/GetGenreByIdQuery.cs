using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetGenreByIdQuery
: IRequest<Result<GenreDto>>
{
    public int Id { get; set; }
}

