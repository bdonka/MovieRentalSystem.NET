using FluentResults;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IMoviePhysicalCopyService
    {
        Task<IEnumerable<MoviePhysicalCopyDto>> GetAllAsync();
        Task<Result<MoviePhysicalCopyDto>> GetByIdAsync(int id, int movieId);
        Task<Result<MoviePhysicalCopyDto>> CreateAsync(MoviePhysicalCopyDto request);
        Task<Result> UpdateAsync(int id, int movieId, MoviePhysicalCopyDto request);
        Task<Result> DeleteAsync(int id, int movieId);
    }
}
