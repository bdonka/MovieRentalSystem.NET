using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreResponse>
{
    private readonly IGenreService _genreService;
    public GetGenreByIdQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<GenreResponse> Handle(
        GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        return await _genreService.GetByIdAsync(request.Id);
    }
}