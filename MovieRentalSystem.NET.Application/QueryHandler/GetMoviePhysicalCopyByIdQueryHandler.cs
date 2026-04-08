using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class GetMoviePhysicalCopyByIdQueryHandler : IRequestHandler<GetMoviePhysicalCopyByIdQuery, MoviePhysicalCopyResponse>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;
    public GetMoviePhysicalCopyByIdQueryHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<MoviePhysicalCopyResponse> Handle(
        GetMoviePhysicalCopyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _moviePhysicalCopyService.GetByIdAsync(request.Id, request.MovieId);
    }
}