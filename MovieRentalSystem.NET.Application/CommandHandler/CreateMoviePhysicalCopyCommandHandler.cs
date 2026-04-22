using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateMoviePhysicalCopyCommandHandler : IRequestHandler<CreateMoviePhysicalCopyCommand, Result<MoviePhysicalCopyDto>>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;

    public CreateMoviePhysicalCopyCommandHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<Result<MoviePhysicalCopyDto>> Handle(
        CreateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new MoviePhysicalCopyDto
        {
            MovieId = request.MovieId,
            Code = request.Code
        };
        var result = await _moviePhysicalCopyService.CreateAsync(createRequest);
        return result;
    }
}