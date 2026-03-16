using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre> CreateAsync(CreateGenreRequest request);
        Task<bool> UpdateAsync(int id, UpdateGenreRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
