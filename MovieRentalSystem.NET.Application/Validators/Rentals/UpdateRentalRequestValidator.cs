using FluentValidation;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;

namespace MovieRentalSystem.NET.Application.Validators.Rentals;

public class UpdateRentalRequestValidator : AbstractValidator<UpdateRentalRequest>
{
    public UpdateRentalRequestValidator()
    {
        RuleFor(x => x.TotalPrice)
            .GreaterThanOrEqualTo(0).WithMessage("TotalPrice must be greater than or equal to 0");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid RentalStatus");

        RuleFor(x => x.RentalStartDate)
            .LessThanOrEqualTo(x => x.DueDate)
            .When(x => x.RentalStartDate.HasValue && x.DueDate.HasValue)
            .WithMessage("RentalStartDate must be earlier than or equal to DueDate");

        RuleFor(x => x.DueDate)
            .LessThanOrEqualTo(x => x.ReturnDate)
            .When(x => x.DueDate.HasValue && x.ReturnDate.HasValue)
            .WithMessage("DueDate must be earlier than or equal to ReturnDate");
    }
}