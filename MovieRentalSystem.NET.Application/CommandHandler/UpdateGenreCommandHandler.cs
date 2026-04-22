using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Result>
{
    private readonly IGenreService _genreService;

    public UpdateGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<Result> Handle(
        UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new GenreDto
        {
            Name = request.Name
        };
        var result = await _genreService.UpdateAsync(request.Id, updateRequest);
        return result;
    }
}