using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public record CreateMovieCommand(
    string Title,
    string Description,
    int ReleaseYear,
    decimal RentalPrice
) : IRequest<Result<MovieDto>>;