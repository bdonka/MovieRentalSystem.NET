using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, Result>
{
    private readonly IGenreService _genreService;

    public DeleteGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<Result> Handle(
        DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var result = await _genreService.DeleteAsync(request.Id);
        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}