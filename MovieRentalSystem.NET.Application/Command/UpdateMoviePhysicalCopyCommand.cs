using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Domain.Enums;

public record UpdateMoviePhysicalCopyCommand(
    int Id,
    int MovieId,
    MovieCopyStatus Status
) : IRequest<Result>
{ }