using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MoviePhysicalCopyService : IMoviePhysicalCopyService
{
    private readonly ApplicationDbContext _context;
    public MoviePhysicalCopyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MoviePhysicalCopyResponse>> GetAllAsync()
    {
        var copies = await _context.MoviePhysicalCopies.ToListAsync();
        return copies.Select(c => c.MapToMoviePhysicalCopyResponse());
    }

    public async Task<MoviePhysicalCopyResponse?> GetByIdAsync(int id, int movieId)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id && c.MovieId == movieId);
        return copy?.MapToMoviePhysicalCopyResponse();
    }

    public async Task<MoviePhysicalCopyResponse> CreateAsync(CreateMoviePhysicalCopyRequest request)
    {
        if (!await _context.Movies.AnyAsync(m => m.Id == request.MovieId))
            throw new InvalidOperationException($"Movie with Id {request.MovieId} does not exist.");

        if (await _context.MoviePhysicalCopies.AnyAsync(c => c.Code == request.Code))
            throw new InvalidOperationException($"Code '{request.Code}' is already used.");

        var copy = new MoviePhysicalCopy
        {
            MovieId = request.MovieId,
            Code = request.Code
        };
        _context.MoviePhysicalCopies.Add(copy);
        await _context.SaveChangesAsync();

        return copy.MapToMoviePhysicalCopyResponse();
    }

    public async Task<bool> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) return false;

        copy.Status = request.Status;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, int movieId)
    {
        var copy = await _context.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) return false;

        _context.MoviePhysicalCopies.Remove(copy);
        await _context.SaveChangesAsync();
        return true;
    }
}