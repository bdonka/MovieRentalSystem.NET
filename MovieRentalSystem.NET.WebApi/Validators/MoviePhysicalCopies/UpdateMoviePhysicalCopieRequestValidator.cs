using FluentValidation;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Enums;

namespace MovieRentalSystem.NET.WebApi.Validators.MoviePhysicalCopies;

public class UpdateMoviePhysicalCopyRequestValidator : AbstractValidator<UpdateMoviePhysicalCopyRequest>
{
    public UpdateMoviePhysicalCopyRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid MovieCopyStatus value");
    }
}