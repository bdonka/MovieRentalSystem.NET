using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMoviePhysicalCopyService
    {
        Task<IEnumerable<MoviePhysicalCopyResponse>> GetAllAsync();
        Task<MoviePhysicalCopyResponse?> GetByIdAsync(int id, int movieId);
        Task<MoviePhysicalCopyResponse> CreateAsync(CreateMoviePhysicalCopyRequest request);
        Task<bool> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request);
        Task<bool> DeleteAsync(int id, int movieId);
    }
}
