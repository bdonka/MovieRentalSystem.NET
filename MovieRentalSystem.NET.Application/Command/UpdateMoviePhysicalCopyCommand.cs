using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Domain.Enums;

public class UpdateMoviePhysicalCopyCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required int MovieId { get; set; }
    public MovieCopyStatus Status { get; set; }
}