using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateMoviePhysicalCopyCommandHandler : IRequestHandler<UpdateMoviePhysicalCopyCommand, Result>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;

    public UpdateMoviePhysicalCopyCommandHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<Result> Handle(
        UpdateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new MoviePhysicalCopyDto
        {
            MovieId = request.MovieId,
        };
        var result = await _moviePhysicalCopyService.UpdateAsync(request.Id, updateRequest);
        return result;
    }
}