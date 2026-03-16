using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMoviePhysicalCopyService
    {
        Task<IEnumerable<MoviePhysicalCopy>> GetAllAsync();
        Task<MoviePhysicalCopy?> GetByIdAsync(int id, int movieId);
        Task<MoviePhysicalCopy> CreateAsync(CreateMoviePhysicalCopyRequest request);
        Task<bool> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request);
        Task<bool> DeleteAsync(int id, int movieId);
    }
}
