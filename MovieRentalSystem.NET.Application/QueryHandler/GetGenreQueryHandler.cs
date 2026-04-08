using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, IEnumerable<GenreResponse>>
{
    private readonly IGenreService _genreService;
    public GetGenreQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<IEnumerable<GenreResponse>> Handle(
        GetGenreQuery request, CancellationToken cancellationToken)
    {
        return await _genreService.GetAllAsync();
    }
}