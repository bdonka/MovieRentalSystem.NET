using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class RentalService : IRentalService
{
    private static readonly List<Rental> _rentals = new();

    public async Task<IEnumerable<RentalResponse>> GetAllAsync()
    {
        return _rentals.Select(r => r.MapToRentalResponse());
    }

    public async Task<RentalResponse?> GetByIdAsync(int id)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == id);
        return rental?.MapToRentalResponse();
    }

    public async Task<RentalResponse> CreateAsync(CreateRentalRequest request)
    {
        var rental = new Rental
        {
            Id = _rentals.Count + 1,
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };

        _rentals.Add(rental);
        return rental.MapToRentalResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateRentalRequest request)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == id);
        if (rental == null) return false;

        rental.RentalStartDate = request.RentalStartDate;
        rental.DueDate = request.DueDate;
        rental.ReturnDate = request.ReturnDate;
        rental.TotalPrice = request.TotalPrice;
        rental.Status = request.Status;

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == id);
        if (rental == null) return false;

        _rentals.Remove(rental);
        return true;
    }
}