using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Enums;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class RentalService : IRentalService
{
    private readonly ApplicationDbContext _context;
    public RentalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RentalResponse>> GetAllAsync()
    {
        var rentals = await _context.Rentals.ToListAsync();
        return rentals.Select(r => r.MapToRentalResponse());
    }

    public async Task<RentalResponse?> GetByIdAsync(int id)
    {
        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        return rental?.MapToRentalResponse();
    }

    public async Task<RentalResponse> CreateAsync(CreateRentalRequest request)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
            throw new InvalidOperationException($"User with Id {request.UserId} does not exist.");

        if (!await _context.MoviePhysicalCopies.AnyAsync(c => c.Id == request.MoviePhysicalCopyId))
            throw new InvalidOperationException($"MoviePhysicalCopy with Id {request.MoviePhysicalCopyId} does not exist.");

        var rental = new Rental
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental.MapToRentalResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateRentalRequest request)
    {
        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null) return false;

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
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        if (rental == null) return false;

        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
        return true;
    }
}