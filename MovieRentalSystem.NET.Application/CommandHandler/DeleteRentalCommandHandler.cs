using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteRentalCommandHandler : IRequestHandler<DeleteRentalCommand, Result>
{
    private readonly IRentalService _rentalService;

    public DeleteRentalCommandHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<Result> Handle(
        DeleteRentalCommand request, CancellationToken cancellationToken)
    {
        var result = await _rentalService.DeleteAsync(request.Id);
        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}