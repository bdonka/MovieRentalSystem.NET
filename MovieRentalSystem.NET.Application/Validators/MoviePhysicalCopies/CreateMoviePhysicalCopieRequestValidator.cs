using FluentValidation;

namespace MovieRentalSystem.NET.Application.Validators.MoviePhysicalCopies;

public class CreateMoviePhysicalCopyRequestValidator : AbstractValidator<CreateMoviePhysicalCopyCommand>
{
    public CreateMoviePhysicalCopyRequestValidator()
    {
        RuleFor(x => x.MovieId)
            .GreaterThan(0).WithMessage("MovieId must be greater than 0");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");
    }
}