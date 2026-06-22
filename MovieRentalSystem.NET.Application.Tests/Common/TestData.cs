using Bogus;
using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Tests.Common;

public static class TestData
{
    // GENRE
    public static Faker<Genre> GenreFaker()
        => new Faker<Genre>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Name, f => f.Commerce.Department());


    public static Genre CreateGenre(int id = 1)
    {
        return new Genre
        {
            Id = id,
            Name = "Action"
        };
    }

    public static List<Genre> Genres(int count)
        => GenreFaker().Generate(count);


    // MOVIE
    public static Faker<Movie> MovieFaker()
        => new Faker<Movie>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.ReleaseYear, f => f.Date.Past(20).Year)
            .RuleFor(x => x.RentalPrice, f => f.Random.Decimal(1, 20))
            .RuleFor(x => x.Genres, _ => new List<Genre>())
            .RuleFor(x => x.PhysicalCopies, _ => new List<MoviePhysicalCopy>());

    public static Movie CreateMovie(bool withRelations = false)
    {
        return CreateMovie(1, withRelations);
    }

    public static Movie CreateMovie(int id, bool withRelations = false)
    {
        var movie = new Movie
        {
            Id = id,
            Title = "Matrix",
            Description = "Sci-fi movie",
            ReleaseYear = 1999,
            RentalPrice = 10m,
            Genres = new List<Genre>(),
            PhysicalCopies = new List<MoviePhysicalCopy>()
        };

        if (withRelations)
        {
            movie.Genres = Genres(2);
            movie.PhysicalCopies = PhysicalCopies(movie, 2);
        }

        return movie;
    }

    // COMMANDS
    public static Faker<CreateMovieCommand> CreateMovieCommandFaker()
        => new Faker<CreateMovieCommand>()
            .CustomInstantiator(f => new CreateMovieCommand(
                f.Commerce.ProductName(),
                f.Lorem.Sentence(),
                f.Date.Past(20).Year,
                f.Random.Decimal(1, 20)));

    public static CreateMovieCommand CreateMovieCommand(string? title = null)
    {
        var f = new Faker();

        return new CreateMovieCommand(
            title ?? f.Commerce.ProductName(),
            f.Lorem.Sentence(),
            f.Date.Past(20).Year,
            f.Random.Decimal(1, 20));
    }

    // MOVIE PHYSICAL COPY
    public static List<MoviePhysicalCopy> PhysicalCopies(Movie movie, int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new MoviePhysicalCopy
            {
                Id = i,
                MovieId = movie.Id,
                Movie = movie,
                Code = $"COPY-{i}",
                Status = MovieCopyStatus.Available
            })
            .ToList();
    }

    public static Faker<MoviePhysicalCopy> MoviePhysicalCopyFaker(Movie movie)
    => new Faker<MoviePhysicalCopy>()
        .RuleFor(x => x.Id, f => f.IndexFaker + 1)
        .RuleFor(x => x.MovieId, movie.Id)
        .RuleFor(x => x.Movie, movie)
        .RuleFor(x => x.Code, f => $"COPY-{f.Random.AlphaNumeric(6)}")
        .RuleFor(x => x.Status, _ => MovieCopyStatus.Available);
}