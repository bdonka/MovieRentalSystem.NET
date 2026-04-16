using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Services;

public class RentalService : IRentalService
{
    private readonly ApplicationDbContext _context;
    public RentalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RentalDto>> GetAllAsync()
    {
        var rentals = await _context.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
            .ToListAsync();
        return rentals.Select(r => r.MapToRentalDto()).ToList();
    }

    public async Task<Result<RentalDto>> GetByIdAsync(int id)
    {
        var rental = await _context.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null)
            return Result.Fail($"Rental with ID {id} not found.");
        return Result.Ok(rental.MapToRentalDto());
    }

    public async Task<Result<RentalDto>> CreateAsync(RentalDto request)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
            return Result.Fail<RentalDto>($"User with Id {request.UserId} does not exist.");

        if (!await _context.MoviePhysicalCopies.AnyAsync(c => c.Id == request.MoviePhysicalCopyId))
            return Result.Fail<RentalDto>($"MoviePhysicalCopy with Id {request.MoviePhysicalCopyId} does not exist.");

        var rental = new Rental
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        var result = await GetByIdAsync(rental.Id);
        return result;
    }

    public async Task<Result> UpdateAsync(int id, RentalDto request)
    {
        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null) 
            return Result.Fail($"Rental with ID {id} not found.");

        rental.RentalStartDate = request.RentalStartDate;
        rental.DueDate = request.DueDate;
        rental.ReturnDate = request.ReturnDate;
        rental.TotalPrice = request.TotalPrice;
        rental.Status = request.Status;

        if (request.ReturnDate != null)
        {
            var copy = await _context.MoviePhysicalCopies
                .FirstOrDefaultAsync(c => c.Id == rental.MoviePhysicalCopyId);

            if (copy != null)
                copy.Status = MovieCopyStatus.Available;
        }

        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null) 
            return Result.Fail($"Rental with ID {id} not found.");

        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}