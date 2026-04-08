using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;
using System.Collections;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, IEnumerable<MovieResponse>>
{
    private readonly IMovieService _movieService;
    public GetMovieQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IEnumerable<MovieResponse>> Handle(
        GetMovieQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetAllAsync();
    }
}