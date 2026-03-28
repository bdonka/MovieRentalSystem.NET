using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMovieGenreService
    {
        Task<IEnumerable<MovieGenreResponse>> GetAllAsync();
        Task<MovieGenreResponse?> GetByIdAsync(int movieId, int genreId);
        Task<MovieGenreResponse> CreateAsync(CreateMovieGenreRequest request);
        Task<bool> DeleteAsync(int movieId, int genreId);
    }
}
