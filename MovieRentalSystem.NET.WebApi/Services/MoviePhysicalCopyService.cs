using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MoviePhysicalCopyService : IMoviePhysicalCopyService
{
    private static readonly List<MoviePhysicalCopy> _copies = new();

    public async Task<IEnumerable<MoviePhysicalCopyResponse>> GetAllAsync()
    {
        return _copies.Select(c => c.MapToMoviePhysicalCopyResponse());
    }

    public async Task<MoviePhysicalCopyResponse?> GetByIdAsync(int id, int movieId)
    {
        var copy = _copies.FirstOrDefault(c => c.Id == id && c.MovieId == movieId);
        return copy?.MapToMoviePhysicalCopyResponse();
    }

    public async Task<MoviePhysicalCopyResponse> CreateAsync(CreateMoviePhysicalCopyRequest request)
    {
        var copy = new MoviePhysicalCopy
        {
            Id = _copies.Count + 1,
            MovieId = request.MovieId,
            Code = request.Code
        };
        _copies.Add(copy);

        return copy.MapToMoviePhysicalCopyResponse();
    }

    public async Task<bool> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request)
    {
        var copy = _copies.FirstOrDefault(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) return false;

        copy.Status = request.Status;
        return true;
    }

    public async Task<bool> DeleteAsync(int id, int movieId)
    {
        var copy = _copies.FirstOrDefault(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) return false;

        _copies.Remove(copy);
        return true;
    }
}