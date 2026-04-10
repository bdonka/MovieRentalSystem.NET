using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, Result<RentalDto>>
{
    private readonly IGenreService _genreService;
    public GetRentalByIdQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<Result<RentalDto>> Handle(
        GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        return await _genreService.GetByIdAsync(request.Id);
    }
}