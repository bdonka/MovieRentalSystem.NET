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
        var copies = await _context.MoviePhysicalCopies
            .Include(m => m.Movie)
            .ToListAsync();
        return copies.Select(c => c.MapToMoviePhysicalCopyDto()).ToList();
    }

    public async Task<Result<MoviePhysicalCopyDto>> GetByIdAsync(int id)
    {
        var copy = await _context.MoviePhysicalCopies.Include(m => m.Movie).FirstOrDefaultAsync(c => c.Id == id);
        if (copy == null) 
            return Result.Fail<MoviePhysicalCopyDto>($"Movie physical copy with Id {id} not found.");
        return Result.Ok(copy.MapToMoviePhysicalCopyDto());
    }

    public async Task<Result<MoviePhysicalCopyDto>> CreateAsync(MoviePhysicalCopyDto request)
    {
        if (await _context.MoviePhysicalCopies.AnyAsync(c => c.Code == request.Code))
            return Result.Fail<MoviePhysicalCopyDto>($"Code '{request.Code}' is already used.");

        var copy = new MoviePhysicalCopy
        {
            MovieId = request.MovieId,
            Code = request.Code
        };
        _context.MoviePhysicalCopies.Add(copy);
        await _context.SaveChangesAsync();

        var result = await GetByIdAsync(copy.Id);
        return result;
    }

    public async Task<Result> UpdateAsync(int id, MoviePhysicalCopyDto request)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id);
        if (copy == null) 
            return Result.Fail($"Movie physical copy with Id {id} not found.");

        copy.Status = request.Status;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id);
        if (copy == null)
            return Result.Fail($"Movie physical copy with Id {id} not found.");

        _context.MoviePhysicalCopies.Remove(copy);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}