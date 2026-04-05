using FluentResults;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreResponse>> GetAllAsync();
        Task<Result<GenreResponse>> GetByIdAsync(int id);
        Task<Result<GenreResponse>> CreateAsync(CreateGenreRequest request);
        Task<Result> UpdateAsync(int id, UpdateGenreRequest request);
        Task<Result> DeleteAsync(int id);
    }
}
