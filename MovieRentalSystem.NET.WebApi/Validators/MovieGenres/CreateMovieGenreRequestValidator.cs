//using FluentValidation;
//using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;

//namespace MovieRentalSystem.NET.WebApi.Validators.MovieGenres;

//public class CreateMovieGenreRequestValidator : AbstractValidator<CreateMovieGenreRequest>
//{
//    public CreateMovieGenreRequestValidator()
//    {
//        RuleFor(x => x.MovieId)
//            .GreaterThan(0).WithMessage("MovieId must be greater than 0");

//        RuleFor(x => x.GenreId)
//            .GreaterThan(0).WithMessage("GenreId must be greater than 0");
//    }
//}