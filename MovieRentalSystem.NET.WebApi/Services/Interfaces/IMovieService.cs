using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieResponse>> GetAllAsync();
        Task<MovieResponse?> GetByIdAsync(int id);
        Task<MovieResponse> CreateAsync(CreateMovieRequest request);
        Task<bool> UpdateAsync(int id, UpdateMovieRequest request);
        Task<bool> DeleteAsync(int id);


        Task<IEnumerable<GenreResponse>?> GetGenresAsync(int movieId);
        Task<bool> AssignGenreAsync(int movieId, int genreId);
        Task<bool> RemoveGenreAsync(int movieId, int genreId);
    }
}
