using FluentResults;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IMoviePhysicalCopyService
    {
        Task<IEnumerable<MoviePhysicalCopyDto>> GetAllAsync();
        Task<Result<MoviePhysicalCopyDto>> GetByIdAsync(int id);
        Task<Result<MoviePhysicalCopyDto>> CreateAsync(MoviePhysicalCopyDto request);
        Task<Result> UpdateAsync(int id, MoviePhysicalCopyDto request);
        Task<Result> DeleteAsync(int id);
    }
}
