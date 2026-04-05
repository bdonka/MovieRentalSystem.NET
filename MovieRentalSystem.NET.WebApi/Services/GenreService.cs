using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext _context;
    public GenreService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<GenreResponse>> GetAllAsync()
    {
        var genres = await _context.Genres.ToListAsync();
        return genres.Select(g => g.MapToGenreResponse());
    }
    public async Task<Result<GenreResponse>> GetByIdAsync(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null) 
            return Result.Fail<GenreResponse>($"Genre with ID {id} not found.");
        return Result.Ok(genre.MapToGenreResponse());
    }
    public async Task<Result<GenreResponse>> CreateAsync(CreateGenreRequest request)
    {
        if (await _context.Genres.AnyAsync(g => g.Name == request.Name))
            return Result.Fail<GenreResponse>($"Genre '{request.Name}' already exists.");
        
        var genre = new Genre
        {
            Name = request.Name
        };
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return Result.Ok(genre.MapToGenreResponse());
    }

    public async Task<Result> UpdateAsync(int id, UpdateGenreRequest request)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null)
            return Result.Fail($"Genre with ID {id} not found.");

        if (await _context.Genres.AnyAsync(g => g.Name == request.Name && g.Id != id))
            return Result.Fail($"Genre '{request.Name}' already exists.");
        genre.Name = request.Name;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<Result> DeleteAsync(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null)
            return Result.Fail($"Genre with ID {id} not found.");
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
