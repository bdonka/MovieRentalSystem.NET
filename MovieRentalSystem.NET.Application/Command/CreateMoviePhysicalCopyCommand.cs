using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
public class CreateMoviePhysicalCopyCommand : IRequest<Result<MoviePhysicalCopyDto>>
{
    public required int MovieId { get; set; }
    public required string Code { get; set; } = null!;
}