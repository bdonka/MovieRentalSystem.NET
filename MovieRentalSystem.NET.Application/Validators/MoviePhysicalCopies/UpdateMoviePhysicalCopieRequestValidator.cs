using FluentValidation;

namespace MovieRentalSystem.NET.Application.Validators.MoviePhysicalCopies;

public class UpdateMoviePhysicalCopyRequestValidator : AbstractValidator<UpdateMoviePhysicalCopyCommand>
{
    public UpdateMoviePhysicalCopyRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.MovieId)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid MovieCopyStatus value");
    }
}