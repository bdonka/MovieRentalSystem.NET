using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MovieGenreService : IMovieGenreService
{
    private static readonly List<MovieGenre> _movieGenres = new();
    public Task<IEnumerable<MovieGenre>> GetAllAsync()
    {
        return Task.FromResult(_movieGenres.AsEnumerable());
    }
    public Task<MovieGenre?> GetByIdAsync(int movieId, int genreId)
    {
        return Task.FromResult(_movieGenres.FirstOrDefault(mg => mg.MovieId == movieId && mg.GenreId == genreId));
    }
    public Task<MovieGenre> CreateAsync(CreateMovieGenreRequest request)
    {
        var movieGenre = new MovieGenre
        {
            MovieId = request.MovieId,
            GenreId = request.GenreId
        };
        _movieGenres.Add(movieGenre);
        return Task.FromResult(movieGenre);
    }

    public Task<bool> DeleteAsync(int movieId, int genreId)
    {
        var movieGenre = _movieGenres.FirstOrDefault(mg => mg.MovieId == movieId && mg.GenreId == genreId);
        if (movieGenre == null) return Task.FromResult(false);
        _movieGenres.Remove(movieGenre);
        return Task.FromResult(true);
    }
}
