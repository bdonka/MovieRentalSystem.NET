using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMovieGenreService
    {
        Task<IEnumerable<MovieGenre>> GetAllAsync();
        Task<MovieGenre?> GetByIdAsync(int movieId, int genreId);
        Task<MovieGenre> CreateAsync(CreateMovieGenreRequest request);
        Task<bool> DeleteAsync(int movieId, int genreId);
    }
}
