using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MovieService : IMovieService
{
    private static readonly List<Movie> _movies = new();

    public async Task<IEnumerable<MovieResponse>> GetAllAsync()
    {
        return _movies.Select(m => m.MapToMovieResponse());
    }

    public async Task<MovieResponse?> GetByIdAsync(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        return movie?.MapToMovieResponse();
    }

    public async Task<MovieResponse> CreateAsync(CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Id = _movies.Count + 1,
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice
        };

        _movies.Add(movie);

        return movie.MapToMovieResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateMovieRequest request)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return false;

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.ReleaseYear = request.ReleaseYear;
        movie.RentalPrice = request.RentalPrice;

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return false;

        _movies.Remove(movie);
        return true;
    }
}