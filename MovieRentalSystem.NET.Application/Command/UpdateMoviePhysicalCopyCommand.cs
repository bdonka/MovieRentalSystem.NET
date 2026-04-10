using FluentResults;
using MediatR;
public class UpdateMoviePhysicalCopyCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required int MovieId { get; set; }
}