using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class RentalQueryHandler : IRequestHandler<RentalQuery, List<Rental>>
{
    private readonly IRentalService _rentalService;
    public RentalQueryHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<List<Rental>> Handle(
        RentalQuery request, CancellationToken cancellationToken)
    {
        var result = await _rentalService.GetAllAsync();

        return result.Select(r => new Rental
        {
            Id = r.Id,
            UserId = r.UserId,
            MoviePhysicalCopyId = r.MoviePhysicalCopyId,
            RentalStartDate = r.RentalStartDate,
            DueDate = r.DueDate
        }).ToList();
    }
}