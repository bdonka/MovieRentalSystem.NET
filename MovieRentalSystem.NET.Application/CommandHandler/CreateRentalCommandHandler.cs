using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, int>
{
    private readonly IRentalService _rentalService;

    public CreateRentalCommandHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<int> Handle(
        CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new CreateRentalRequest
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };
        var result = await _rentalService.CreateAsync(createRequest);

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result.Value.Id;
    }
}