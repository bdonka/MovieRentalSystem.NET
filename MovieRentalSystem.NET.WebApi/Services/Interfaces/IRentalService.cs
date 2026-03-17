using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalResponse>> GetAllAsync();
        Task<RentalResponse?> GetByIdAsync(int id);
        Task<RentalResponse> CreateAsync(CreateRentalRequest request);
        Task<bool> UpdateAsync(int id, UpdateRentalRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
