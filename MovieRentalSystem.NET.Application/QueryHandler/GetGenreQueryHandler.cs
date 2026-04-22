using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, IEnumerable<GenreDto>>
{
    private readonly IGenreService _genreService;
    public GetGenreQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<IEnumerable<GenreDto>> Handle(
        GetGenreQuery request, CancellationToken cancellationToken)
    {
        return await _genreService.GetAllAsync();
    }
}