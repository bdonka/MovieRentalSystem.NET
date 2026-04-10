using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;
    public MovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieDto>> GetAllAsync()
    {
        var movies = await _context.Movies.ToListAsync();
        return movies.Select(m => m.MapToMovieDto());
    }

    public async Task<Result<MovieDto>> GetByIdAsync(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) 
            return Result.Fail<MovieDto>($"Movie with ID {id} not found.");
        return Result.Ok(movie.MapToMovieDto());
    }

    public async Task<Result<MovieDto>> CreateAsync(MovieDto request)
    {
        if (await _context.Movies.AnyAsync(m => m.Title == request.Title))
            return Result.Fail<MovieDto>($"Movie '{request.Title}' already exists.");

        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return Result.Ok(movie.MapToMovieDto());
    }

    public async Task<Result> UpdateAsync(int id, MovieDto request)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) 
            return Result.Fail($"Movie with ID {id} not found.");

        if (await _context.Movies.AnyAsync(m => m.Title == request.Title && m.Id != id))
            return Result.Fail($"Movie '{request.Title}' already exists.");

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.ReleaseYear = request.ReleaseYear;
        movie.RentalPrice = request.RentalPrice;

        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) 
            return Result.Fail($"Movie with ID {id} not found.");

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }



    public async Task<Result<IEnumerable<GenreDto>>> GetGenresAsync(int movieId)
    {
        var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
        {
            return Result.Fail<IEnumerable<GenreDto>>($"Movie with Id {movieId} not found.");
        }

        var genres = movie.Genres.ToList().Select(g => g.MapToGenreDto());
        return Result.Ok(genres);
    }

    public async Task<Result> AssignGenreAsync(int movieId, int genreId) 
    { 
        var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);
        var genre = await _context.Genres.FindAsync(genreId);
        
        if (movie == null) 
            return Result.Fail($"Movie with Id {movieId} not found.");

        if (genre == null) 
            return Result.Fail($"Genre with Id {genreId} not found.");

        if (!movie.Genres.Contains(genre))
            movie.Genres.Add(genre);

        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> RemoveGenreAsync(int movieId, int genreId)
    {
        var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);
        var genre = await _context.Genres.FindAsync(genreId);

        if (movie == null)
            return Result.Fail($"Movie with Id {movieId} not found.");

        if (genre == null)
            return Result.Fail($"Genre with Id {genreId} not found.");

        if (movie.Genres.Contains(genre))
            movie.Genres.Remove(genre);

        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}