using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<MovieDto>>
{
    private readonly IMovieService _movieService;

    public CreateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result<MovieDto>> Handle(
        CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new MovieDto
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice,
        };
        var result = await _movieService.CreateAsync(createRequest);
        return result;
    }
}