using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MovieService : IMovieService
{
    private static readonly List<Movie> _movies = new();

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<Movie?> GetByIdAsync(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie> CreateAsync(CreateMovieRequest request)
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

        return Task.FromResult(movie);
    }

    public Task<bool> UpdateAsync(int id, UpdateMovieRequest request)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
            return Task.FromResult(false);

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.ReleaseYear = request.ReleaseYear;
        movie.RentalPrice = request.RentalPrice;

        return Task.FromResult(true);
    }
    public Task<bool> DeleteAsync(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
            return Task.FromResult(false);

        _movies.Remove(movie);

        return Task.FromResult(true);
    }
}
