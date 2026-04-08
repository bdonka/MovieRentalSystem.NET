using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse>
{
    private readonly IGenreService _genreService;
    public GetMovieByIdQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<MovieResponse> Handle(
        GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        return await _genreService.GetByIdAsync(request.Id);
    }
}