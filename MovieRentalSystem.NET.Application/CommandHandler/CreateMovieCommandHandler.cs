using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, int>
{
    private readonly IMovieService _movieService;

    public CreateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<int> Handle(
        CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new CreateMovieRequest
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice
        };
        var result = await _movieService.CreateAsync(createRequest);

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result.Value.Id;
    }
}