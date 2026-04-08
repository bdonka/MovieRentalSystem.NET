using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class GetMoviePhysicalCopyQueryHandler : IRequestHandler<GetMoviePhysicalCopyQuery, IEnumerable<MoviePhysicalCopyResponse>>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;
    public GetMoviePhysicalCopyQueryHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<IEnumerable<MoviePhysicalCopyResponse>> Handle(
        GetMoviePhysicalCopyQuery request, CancellationToken cancellationToken)
    {
        return await _moviePhysicalCopyService.GetAllAsync();
    }
}