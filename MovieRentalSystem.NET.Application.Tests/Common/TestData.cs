using Bogus;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Tests.Common;

public static class TestData
{
    public static DateTime FixedNow => new DateTime(2025, 01, 01);

    // GENRE
    public static Faker<Genre> GenreFaker()
        => new Faker<Genre>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Name, f => f.Commerce.Department());

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

    public static MoviePhysicalCopy CreateMovieCopy(
    int id,
    Movie movie,
    MovieCopyStatus status = MovieCopyStatus.Available)
    {
        return new MoviePhysicalCopy
        {
            Id = id,
            MovieId = movie.Id,
            Movie = movie,
            Code = $"COPY-{id}",
            Status = status
        };
    }

    // USER
    public static User CreateUser(string? id = null)
    {
        var faker = new Faker();
        var email = faker.Internet.Email();
        var userName = faker.Internet.UserName();

        return new User
        {
            Id = id ?? Guid.NewGuid().ToString(),
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = userName.ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString(),
            DateRegistered = FixedNow,
            Rentals = new List<Rental>()
        };
    }

    // RENTAL
    public static Rental CreateRental(
        int id,
        User user,
        MoviePhysicalCopy copy,
        RentalStatus status = RentalStatus.Active)
    {
        return new Rental
        {
            Id = id,
            UserId = user.Id,
            User = user,
            MoviePhysicalCopyId = copy.Id,
            MoviePhysicalCopy = copy,
            OrderDate = FixedNow,
            RentalStartDate = FixedNow,
            DueDate = FixedNow.AddDays(7),
            Status = status,
            TotalPrice = 50m
        };
    }
}