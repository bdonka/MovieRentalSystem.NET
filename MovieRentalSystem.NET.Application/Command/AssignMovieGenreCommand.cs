using FluentResults;
using MediatR;

public class AssignMovieGenreCommand : IRequest<Result>
{
    public required int MovieId { get; set; }
    public required int GenreId { get; set; }
}