using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class MoviePhysicalCopyService : IMoviePhysicalCopyService
{
    private static readonly List<MoviePhysicalCopy> _copies = new();
    public Task<IEnumerable<MoviePhysicalCopy>> GetAllAsync()
    {
        return Task.FromResult(_copies.AsEnumerable());
    }
    public Task<MoviePhysicalCopy?> GetByIdAsync(int id, int movieId)
    {
        return Task.FromResult(_copies.FirstOrDefault(c => c.Id == id && c.MovieId == movieId));
    }
    public Task<MoviePhysicalCopy> CreateAsync(CreateMoviePhysicalCopyRequest request)
    {
        var copy = new MoviePhysicalCopy
        {
            Id = _copies.Count + 1,
            MovieId = request.MovieId,
            Code = request.Code,
        };
        _copies.Add(copy);
        return Task.FromResult(copy);
    }
    public Task<bool> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request)
    {
        var copy = _copies.FirstOrDefault(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) return Task.FromResult(false);
        copy.Status = request.Status;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id, int movieId)
    {
        var copy = _copies.FirstOrDefault(c => c.Id == id && c.MovieId == movieId);
        if (copy == null) return Task.FromResult(false);
        _copies.Remove(copy);
        return Task.FromResult(true);
    }
}
