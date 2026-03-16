using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class GenreService : IGenreService
{
    private static readonly List<Genre> _genres = new();

    public Task<IEnumerable<Genre>> GetAllAsync()
    {
        return Task.FromResult(_genres.AsEnumerable());
    }
    public Task<Genre?> GetByIdAsync(int id)
    {
        return Task.FromResult(_genres.FirstOrDefault(g => g.Id == id));
    }
    public Task<Genre> CreateAsync(CreateGenreRequest request)
    {
        var genre = new Genre
        {
            Id = _genres.Count + 1,
            Name = request.Name
        };
        _genres.Add(genre);
        return Task.FromResult(genre);
    }

    public Task<bool> UpdateAsync(int id, UpdateGenreRequest request)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == id);
        if (genre == null) return Task.FromResult(false);
        genre.Name = request.Name;
        return Task.FromResult(true);
    }
    public Task<bool> DeleteAsync(int id)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == id);
        if (genre == null) return Task.FromResult(false);
        _genres.Remove(genre);
        return Task.FromResult(true);
    }
}
