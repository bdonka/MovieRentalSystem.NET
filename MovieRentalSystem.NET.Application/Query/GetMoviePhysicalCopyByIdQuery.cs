using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public record GetMoviePhysicalCopyByIdQuery(int Id)
: IRequest<Result<MoviePhysicalCopyDto>>
{ }

