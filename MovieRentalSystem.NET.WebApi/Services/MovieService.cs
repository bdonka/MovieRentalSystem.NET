using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;
    public MovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieResponse>> GetAllAsync()
    {
        var movies = await _context.Movies.ToListAsync();
        return movies.Select(m => m.MapToMovieResponse());
    }

    public async Task<MovieResponse?> GetByIdAsync(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        return movie?.MapToMovieResponse();
    }

    public async Task<MovieResponse> CreateAsync(CreateMovieRequest request)
    {
        if (await _context.Movies.AnyAsync(m => m.Title == request.Title))
            throw new InvalidOperationException($"Movie '{request.Title}' already exists.");

        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return movie.MapToMovieResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateMovieRequest request)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return false;

        if (await _context.Movies.AnyAsync(m => m.Title == request.Title && m.Id != id))
            throw new InvalidOperationException($"Movie '{request.Title}' already exists.");

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.ReleaseYear = request.ReleaseYear;
        movie.RentalPrice = request.RentalPrice;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }



    public async Task<IEnumerable<GenreResponse>?> GetGenresAsync(int movieId)
    {
        var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
        {
            return null;
        }

        return movie.Genres.ToList().Select(g => g.MapToGenreResponse());
    }

    public async Task<bool> AssignGenreAsync(int movieId, int genreId) 
    { 
        var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);
        var genre = await _context.Genres.FindAsync(genreId);
        
        if (movie == null || genre == null) return false;

        if (!movie.Genres.Contains(genre))
            movie.Genres.Add(genre);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveGenreAsync(int movieId, int genreId)
    {
        var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);
        var genre = await _context.Genres.FindAsync(genreId);

        if (movie == null || genre == null) return false;

        if (movie.Genres.Contains(genre))
            movie.Genres.Remove(genre);

        await _context.SaveChangesAsync();
        return true;
    }
}