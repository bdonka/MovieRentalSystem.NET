using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, Result>
{
    private readonly IMovieService _movieService;

    public DeleteMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Result> Handle(
        DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var result = await _movieService.DeleteAsync(request.Id);
        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}