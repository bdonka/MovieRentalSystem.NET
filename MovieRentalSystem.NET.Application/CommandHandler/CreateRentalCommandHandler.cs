using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Result<RentalDto>>
{
    private readonly IRentalService _rentalService;

    public CreateRentalCommandHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<Result<RentalDto>> Handle(
        CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new RentalDto
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };
        var result = await _rentalService.CreateAsync(createRequest);
        return result;
    }
}