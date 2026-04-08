namespace MovieRentalSystem.NET.Application.Validators.Movies;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.ReleaseYear)
            .InclusiveBetween(1888, DateTime.UtcNow.Year)
            .WithMessage($"ReleaseYear must be between 1888 and {DateTime.UtcNow.Year}");

        RuleFor(x => x.RentalPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("RentalPrice must be greater than or equal to 0");
    }
}