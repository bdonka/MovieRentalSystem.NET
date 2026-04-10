using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Services;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext _context;
    public GenreService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var genres = await _context.Genres.ToListAsync();
        return genres.Select(g => g.MapToGenreDto());
    }
    public async Task<Result<GenreDto>> GetByIdAsync(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null) 
            return Result.Fail<GenreDto>($"Genre with ID {id} not found.");
        return Result.Ok(genre.MapToGenreDto());
    }
    public async Task<Result<GenreDto>> CreateAsync(GenreDto request)
    {
        if (await _context.Genres.AnyAsync(g => g.Name == request.Name))
            return Result.Fail<GenreDto>($"Genre '{request.Name}' already exists.");
        
        var genre = new Genre
        {
            Name = request.Name
        };
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return Result.Ok(genre.MapToGenreDto());
    }

    public async Task<Result> UpdateAsync(int id, GenreDto request)
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
