using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateMoviePhysicalCopyCommandHandler : IRequestHandler<CreateMoviePhysicalCopyCommand, int>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;

    public CreateMoviePhysicalCopyCommandHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<int> Handle(
        CreateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new CreateMoviePhysicalCopyRequest
        {
            MovieId = request.MovieId,
            Code = request.Code
        };
        var result = await _moviePhysicalCopyService.CreateAsync(createRequest);

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result.Value.Id;
    }
}