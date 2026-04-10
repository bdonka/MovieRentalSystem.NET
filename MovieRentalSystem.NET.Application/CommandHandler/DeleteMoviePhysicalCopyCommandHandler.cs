using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteMoviePhysicalCopyCommandHandler : IRequestHandler<DeleteMoviePhysicalCopyCommand, Result>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;

    public DeleteMoviePhysicalCopyCommandHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<Result> Handle(
        DeleteMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        var result = await _moviePhysicalCopyService.DeleteAsync(request.Id, request.MovieId);
        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}