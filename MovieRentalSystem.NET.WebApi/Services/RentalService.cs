using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class RentalService : IRentalService
{
    private static readonly List<Rental> _rentals = new();
    public Task<IEnumerable<Rental>> GetAllAsync()
    {
        return Task.FromResult(_rentals.AsEnumerable());
    }

    public Task<Rental?> GetByIdAsync(int id)
    {
        return Task.FromResult(_rentals.FirstOrDefault(r => r.Id == id));
    }

    public Task<Rental> CreateAsync(CreateRentalRequest request)
    {
        var rental = new Rental
        {
            Id = _rentals.Count + 1,
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate,
        };
        _rentals.Add(rental);
        return Task.FromResult(rental);
    }

    public Task<bool> UpdateAsync(int id, UpdateRentalRequest request)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == id);
        if (rental == null) return Task.FromResult(false);

        rental.RentalStartDate = request.RentalStartDate;
        rental.DueDate = request.DueDate;
        rental.ReturnDate = request.ReturnDate;
        rental.TotalPrice = request.TotalPrice;
        rental.Status = request.Status;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == id);
        if (rental == null) return Task.FromResult(false);
        _rentals.Remove(rental);
        return Task.FromResult(true);
    }






}
