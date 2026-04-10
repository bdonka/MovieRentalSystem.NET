using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class GetMoviePhysicalCopyByIdQueryHandler : IRequestHandler<GetMoviePhysicalCopyByIdQuery, Result<MoviePhysicalCopyDto>>
{
    private readonly IMoviePhysicalCopyService _moviePhysicalCopyService;
    public GetMoviePhysicalCopyByIdQueryHandler(IMoviePhysicalCopyService moviePhysicalCopyService)
    {
        _moviePhysicalCopyService = moviePhysicalCopyService;
    }

    public async Task<Result<MoviePhysicalCopyDto>> Handle(
        GetMoviePhysicalCopyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _moviePhysicalCopyService.GetByIdAsync(request.Id, request.MovieId);
    }
}