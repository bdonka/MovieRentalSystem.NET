using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, IEnumerable<MovieDto>>
{
    private readonly IMovieService _movieService;
    public GetMovieQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IEnumerable<MovieDto>> Handle(
        GetMovieQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetAllAsync();
    }
}