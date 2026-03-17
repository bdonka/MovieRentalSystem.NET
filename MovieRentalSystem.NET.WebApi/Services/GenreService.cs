using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class GenreService : IGenreService
{
    private static readonly List<Genre> _genres = new();

    public async Task<IEnumerable<GenreResponse>> GetAllAsync()
    {
        return _genres.Select(g => g.MapToGenreResponse());
    }
    public async Task<GenreResponse?> GetByIdAsync(int id)
    {
        return _genres
            .Select(g => g.MapToGenreResponse())
            .FirstOrDefault(g => g.Id == id);
    }
    public async Task<GenreResponse> CreateAsync(CreateGenreRequest request)
    {
        var genre = new Genre
        {
            Id = _genres.Count + 1,
            Name = request.Name
        };
        _genres.Add(genre);

        return genre.MapToGenreResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateGenreRequest request)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == id);
        if (genre == null) return false;
        genre.Name = request.Name;
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == id);
        if (genre == null) return false;
        _genres.Remove(genre);
        return true;
    }
}
