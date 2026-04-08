namespace MovieRentalSystem.NET.Application.Validators.Genres;

public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
{
    public UpdateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required")
            .MaximumLength(100).WithMessage("Genre name must not exceed 100 characters");
    }
}