using FluentResults;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<Result<MovieDto>> GetByIdAsync(int id);
        Task<Result<MovieDto>> CreateAsync(MovieDto request);
        Task<Result> UpdateAsync(int id, MovieDto request);
        Task<Result> DeleteAsync(int id);

        Task<Result> AssignGenreAsync(int movieId, int genreId);
        Task<Result> RemoveGenreAsync(int movieId, int genreId);
    }
}
