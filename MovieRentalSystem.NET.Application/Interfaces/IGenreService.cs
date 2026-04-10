using FluentResults;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<Result<GenreDto>> GetByIdAsync(int id);
        Task<Result<GenreDto>> CreateAsync(GenreDto request);
        Task<Result> UpdateAsync(int id, GenreDto request);
        Task<Result> DeleteAsync(int id);
    }
}
