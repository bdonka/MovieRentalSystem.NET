using FluentValidation;

namespace MovieRentalSystem.NET.Application.Validators.Rentals;

public class CreateRentalRequestValidator : AbstractValidator<CreateRentalCommand>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0");

        RuleFor(x => x.MoviePhysicalCopyId)
            .GreaterThan(0).WithMessage("MoviePhysicalCopyId must be greater than 0");

        RuleFor(x => x.RentalStartDate)
            .LessThanOrEqualTo(x => x.DueDate)
            .When(x => x.RentalStartDate.HasValue && x.DueDate.HasValue)
            .WithMessage("RentalStartDate must be earlier than or equal to DueDate");
    }
}