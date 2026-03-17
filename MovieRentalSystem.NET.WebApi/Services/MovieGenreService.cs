using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MovieGenreService : IMovieGenreService
{
    private static readonly List<MovieGenre> _movieGenres = new();

    public async Task<IEnumerable<MovieGenreResponse>> GetAllAsync()
    {
        return _movieGenres.Select(mg => mg.MapToMovieGenreResponse());
    }

    public async Task<MovieGenreResponse?> GetByIdAsync(int movieId, int genreId)
    {
        var movieGenre = _movieGenres.FirstOrDefault(mg => mg.MovieId == movieId && mg.GenreId == genreId);
        return movieGenre?.MapToMovieGenreResponse();
    }

    public async Task<MovieGenreResponse> CreateAsync(CreateMovieGenreRequest request)
    {
        var movieGenre = new MovieGenre
        {
            MovieId = request.MovieId,
            GenreId = request.GenreId
        };
        _movieGenres.Add(movieGenre);

        return movieGenre.MapToMovieGenreResponse();
    }

    public async Task<bool> DeleteAsync(int movieId, int genreId)
    {
        var movieGenre = _movieGenres.FirstOrDefault(mg => mg.MovieId == movieId && mg.GenreId == genreId);
        if (movieGenre == null) return false;

        _movieGenres.Remove(movieGenre);
        return true;
    }
}