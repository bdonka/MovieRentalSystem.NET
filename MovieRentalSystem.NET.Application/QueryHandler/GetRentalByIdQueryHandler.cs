using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, Result<RentalDto>>
{
    private readonly IRentalService _rentalService;
    public GetRentalByIdQueryHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<Result<RentalDto>> Handle(
        GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        return await _rentalService.GetByIdAsync(request.Id);
    }
}