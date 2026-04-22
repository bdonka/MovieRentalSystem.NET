using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class RemoveMovieGenreCommandHandler : IRequestHandler<RemoveMovieGenreCommand, Result>
{
    private readonly IMovieService _movieService;

    public RemoveMovieGenreCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result> Handle(
        RemoveMovieGenreCommand request, CancellationToken cancellationToken)
    {
        var result = await _movieService.RemoveGenreAsync(request.MovieId, request.GenreId);
        return result;
    }
}