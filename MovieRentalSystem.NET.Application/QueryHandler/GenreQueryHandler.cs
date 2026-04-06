using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class GenreQueryHandler : IRequestHandler<GenreQuery, List<Genre>>
{
    private readonly IGenreService _genreService;
    public GenreQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<List<Genre>> Handle(
        GenreQuery request, CancellationToken cancellationToken)
    {
        var result = await _genreService.GetAllAsync();

        return result.Select(r => new Genre
        {
            Id = r.Id,
            Name = r.Name,
        }).ToList();
    }
}