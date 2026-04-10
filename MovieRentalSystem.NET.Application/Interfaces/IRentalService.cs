using FluentResults;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalDto>> GetAllAsync();
        Task<Result<RentalDto>> GetByIdAsync(int id);
        Task<Result<RentalDto>> CreateAsync(RentalDto request);
        Task<Result> UpdateAsync(int id, RentalDto request);
        Task<Result> DeleteAsync(int id);
    }
}
