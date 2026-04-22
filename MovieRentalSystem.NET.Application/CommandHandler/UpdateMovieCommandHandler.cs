using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Result>
{
    private readonly IMovieService _movieService;

    public UpdateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result> Handle(
        UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new MovieDto
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice,
        };
        var result = await _movieService.UpdateAsync(request.Id, updateRequest);
        return result;
    }
}