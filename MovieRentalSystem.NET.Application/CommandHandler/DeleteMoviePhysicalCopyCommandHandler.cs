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
        var result = await _moviePhysicalCopyService.DeleteAsync(request.Id);
        return result;
    }
}