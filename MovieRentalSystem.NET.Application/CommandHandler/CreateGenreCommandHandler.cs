using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, int>
{
    private readonly IGenreService _genreService;

    public CreateGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<int> Handle(
        CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new CreateGenreRequest
        {
            Name = request.Name
        };
        var result = await _genreService.CreateAsync(createRequest);

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result.Value.Id;
    }
}