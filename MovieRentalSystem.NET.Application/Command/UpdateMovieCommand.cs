using FluentResults;
using MediatR;

public record UpdateMovieCommand(int Id,
    string Title,
    string Description,
    int ReleaseYear,
    decimal RentalPrice) : IRequest<Result>;