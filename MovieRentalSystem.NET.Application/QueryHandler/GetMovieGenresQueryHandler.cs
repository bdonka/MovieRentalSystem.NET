using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
public class GetMovieGenresQueryHandler : IRequestHandler<GetMovieGenresQuery, Result<IEnumerable<GenreDto>>>
{
    private readonly IMovieService _movieService;
    public GetMovieGenresQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result<IEnumerable<GenreDto>>> Handle(
        GetMovieGenresQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetGenresAsync(request.MovieId);
    }
}