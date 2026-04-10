using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieDto>>
{
    private readonly IMovieService _movieService;
    public GetMovieByIdQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result<MovieDto>> Handle(
        GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetByIdAsync(request.Id);
    }
}