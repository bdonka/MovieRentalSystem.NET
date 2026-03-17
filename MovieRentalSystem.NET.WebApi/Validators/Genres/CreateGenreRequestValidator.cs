using FluentValidation;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;

namespace MovieRentalSystem.NET.WebApi.Validators.Genres;

public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public CreateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required")
            .MaximumLength(100).WithMessage("Genre name must not exceed 100 characters");
    }
}
