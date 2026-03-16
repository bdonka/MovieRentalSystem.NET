using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int id);
        Task<Rental> CreateAsync(CreateRentalRequest request);
        Task<bool> UpdateAsync(int id, UpdateRentalRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
