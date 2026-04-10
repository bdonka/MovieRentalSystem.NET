using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class AssignMovieGenreCommandHandler : IRequestHandler<AssignMovieGenreCommand, Result>
{
    private readonly IMovieService _movieService;

    public AssignMovieGenreCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result> Handle(
        AssignMovieGenreCommand request, CancellationToken cancellationToken)
    {
        var result = await _movieService.AssignGenreAsync(request.MovieId, request.GenreId);
        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}