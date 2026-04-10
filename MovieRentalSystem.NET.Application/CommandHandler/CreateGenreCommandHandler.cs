using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Result<GenreDto>>
{
    private readonly IGenreService _genreService;

    public CreateGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<Result<GenreDto>> Handle(
        CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new GenreDto
        {
            Name = request.Name
        };
        var result = await _genreService.CreateAsync(createRequest);

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}