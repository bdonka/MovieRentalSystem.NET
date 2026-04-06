using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class MovieQueryHandler : IRequestHandler<MovieQuery, List<Movie>>
{
    private readonly IMovieService _movieService;
    public MovieQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<List<Movie>> Handle(
        MovieQuery request, CancellationToken cancellationToken)
    {
        var result = await _movieService.GetAllAsync();

        return result.Select(r => new Movie
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            ReleaseYear = r.ReleaseYear,
            RentalPrice = r.RentalPrice
        }).ToList();
    }
}