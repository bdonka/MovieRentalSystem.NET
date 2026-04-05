using FluentResults;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IMoviePhysicalCopyService
    {
        Task<IEnumerable<MoviePhysicalCopyResponse>> GetAllAsync();
        Task<Result<MoviePhysicalCopyResponse>> GetByIdAsync(int id, int movieId);
        Task<Result<MoviePhysicalCopyResponse>> CreateAsync(CreateMoviePhysicalCopyRequest request);
        Task<Result> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request);
        Task<Result> DeleteAsync(int id, int movieId);
    }
}
