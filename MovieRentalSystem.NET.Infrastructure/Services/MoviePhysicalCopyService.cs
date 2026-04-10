using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Services;

public class MoviePhysicalCopyService : IMoviePhysicalCopyService
{
    private readonly ApplicationDbContext _context;
    public MoviePhysicalCopyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MoviePhysicalCopyDto>> GetAllAsync()
    {
        var copies = await _context.MoviePhysicalCopies.ToListAsync();
        return copies.Select(c => c.MapToMoviePhysicalCopyDto());
    }

    public async Task<Result<MoviePhysicalCopyDto>> GetByIdAsync(int id, int movieId)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) 
            return Result.Fail<MoviePhysicalCopyDto>($"Movie physical copy with Id {id} for MovieId {movieId} not found.");
        return Result.Ok(copy.MapToMoviePhysicalCopyDto());
    }

    public async Task<Result<MoviePhysicalCopyDto>> CreateAsync(MoviePhysicalCopyDto request)
    {
        if (!await _context.Movies.AnyAsync(m => m.Id == request.MovieId))
            Result.Fail<MoviePhysicalCopyDto>($"Movie with Id {request.MovieId} does not exist.");

        if (await _context.MoviePhysicalCopies.AnyAsync(c => c.Code == request.Code))
            Result.Fail<MoviePhysicalCopyDto>($"Code '{request.Code}' is already used.");

        var copy = new MoviePhysicalCopy
        {
            MovieId = request.MovieId,
            Code = request.Code
        };
        _context.MoviePhysicalCopies.Add(copy);
        await _context.SaveChangesAsync();

        return Result.Ok(copy.MapToMoviePhysicalCopyDto());
    }

    public async Task<Result> UpdateAsync(int id, int movieId, MoviePhysicalCopyDto request)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) 
            return Result.Fail($"Movie physical copy with Id {id} for MovieId {movieId} not found.");

        copy.Status = request.Status;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id, int movieId)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id && c.MovieId == movieId);
        if (copy == null)
            return Result.Fail($"Movie physical copy with Id {id} for MovieId {movieId} not found.");

        _context.MoviePhysicalCopies.Remove(copy);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}