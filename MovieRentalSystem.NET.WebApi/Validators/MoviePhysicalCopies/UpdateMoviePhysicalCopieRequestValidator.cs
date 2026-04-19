using FluentValidation;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;

namespace MovieRentalSystem.NET.Application.Validators.MoviePhysicalCopies;

public class UpdateMoviePhysicalCopyRequestValidator : AbstractValidator<UpdateMoviePhysicalCopyRequest>
{
    public UpdateMoviePhysicalCopyRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid MovieCopyStatus value");
    }
}