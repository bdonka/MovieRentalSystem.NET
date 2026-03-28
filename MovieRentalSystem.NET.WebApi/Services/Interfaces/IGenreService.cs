using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreResponse>> GetAllAsync();
        Task<GenreResponse?> GetByIdAsync(int id);
        Task<GenreResponse> CreateAsync(CreateGenreRequest request);
        Task<bool> UpdateAsync(int id, UpdateGenreRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
