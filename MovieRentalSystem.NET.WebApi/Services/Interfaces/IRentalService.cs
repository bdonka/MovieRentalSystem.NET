using FluentResults;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalResponse>> GetAllAsync();
        Task<Result<RentalResponse>> GetByIdAsync(int id);
        Task<Result<RentalResponse>> CreateAsync(CreateRentalRequest request);
        Task<Result> UpdateAsync(int id, UpdateRentalRequest request);
        Task<Result> DeleteAsync(int id);
    }
}
