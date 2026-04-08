using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, IEnumerable<RentalResponse>>
{
    private readonly IRentalService _rentalService;
    public GetRentalQueryHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<IEnumerable<RentalResponse>> Handle(
        GetRentalQuery request, CancellationToken cancellationToken)
    {
        return await _rentalService.GetAllAsync();
    }
}