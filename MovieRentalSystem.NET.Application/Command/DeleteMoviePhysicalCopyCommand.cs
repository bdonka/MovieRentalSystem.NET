using FluentResults;
using MediatR;
public class DeleteMoviePhysicalCopyCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required int MovieId { get; set; }
}