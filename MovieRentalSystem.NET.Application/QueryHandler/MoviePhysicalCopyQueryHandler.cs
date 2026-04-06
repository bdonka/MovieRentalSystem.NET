using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class MoviePhysicalCopyQueryHandler : IRequestHandler<MoviePhysicalCopyQuery, List<MoviePhysicalCopy>>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;
    public MoviePhysicalCopyQueryHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<List<MoviePhysicalCopy>> Handle(
        MoviePhysicalCopyQuery request, CancellationToken cancellationToken)
    {
        var result = await _moviePhysicalCopyService.GetAllAsync();

        return result.Select(r => new MoviePhysicalCopy
        {
            Id = r.Id,
            MovieId = r.MovieId,
            Code = r.Code
        }).ToList();
    }
}