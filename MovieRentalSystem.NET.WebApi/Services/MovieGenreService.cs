using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MovieGenreService : IMovieGenreService
{
    private readonly ApplicationDbContext _context;
    public MovieGenreService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieGenreResponse>> GetAllAsync()
    {
        return await _context.MovieGenres.Select(mg => mg.MapToMovieGenreResponse()).ToListAsync();
    }

    public async Task<MovieGenreResponse?> GetByIdAsync(int movieId, int genreId)
    {
        var movieGenre = await _context.MovieGenres.FirstOrDefaultAsync(mg => mg.MovieId == movieId && mg.GenreId == genreId);
        return movieGenre?.MapToMovieGenreResponse();
    }

    public async Task<MovieGenreResponse> CreateAsync(CreateMovieGenreRequest request)
    {
        if (!await _context.Movies.AnyAsync(m => m.Id == request.MovieId))
            throw new InvalidOperationException($"Movie with Id {request.MovieId} does not exist.");

        if (!await _context.Genres.AnyAsync(g => g.Id == request.GenreId))
            throw new InvalidOperationException($"Genre with Id {request.GenreId} does not exist.");

        if (await _context.MovieGenres.AnyAsync(mg => mg.MovieId == request.MovieId && mg.GenreId == request.GenreId))
            throw new InvalidOperationException("This MovieGenre already exists.");

        var movieGenre = new MovieGenre
        {
            MovieId = request.MovieId,
            GenreId = request.GenreId
        };
        _context.MovieGenres.Add(movieGenre);
        await _context.SaveChangesAsync();
        return movieGenre.MapToMovieGenreResponse();
    }

    public async Task<bool> DeleteAsync(int movieId, int genreId)
    {
        var movieGenre = await _context.MovieGenres.FirstOrDefaultAsync(mg => mg.MovieId == movieId && mg.GenreId == genreId);
        if (movieGenre == null) return false;

        _context.MovieGenres.Remove(movieGenre);
        await _context.SaveChangesAsync();
        return true;
    }
}