using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie> CreateAsync(CreateMovieRequest request);
        Task<bool> UpdateAsync(int id, UpdateMovieRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
