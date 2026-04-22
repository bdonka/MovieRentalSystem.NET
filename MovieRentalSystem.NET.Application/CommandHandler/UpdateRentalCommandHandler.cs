using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand, Result>
{
    private readonly IRentalService _rentalService;

    public UpdateRentalCommandHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<Result> Handle(
        UpdateRentalCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new RentalDto
        {
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate,
            ReturnDate = request.ReturnDate,
            TotalPrice = request.TotalPrice,
            Status = request.Status
        };
        var result = await _rentalService.UpdateAsync(request.Id, updateRequest);
        return result;
    }
}