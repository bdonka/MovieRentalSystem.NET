using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public record GetMovieByIdQuery(int Id)
: IRequest<Result<MovieDto>>;

