using Bogus;
using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Tests.Common;

public static class TestData
{
    public static DateTime FixedNow => new(2025, 01, 01);

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
    public static Movie CreateMovie(
        bool withRelations = false)
    {
        return CreateMovie(1, withRelations);
    }


    public static Movie CreateMovie(
        int id,
        bool withRelations = false)
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
            .CustomInstantiator(f =>
                new CreateMovieCommand(
                    f.Commerce.ProductName(),
                    f.Lorem.Sentence(),
                    f.Date.Past(20).Year,
                    f.Random.Decimal(1, 20)));

    public static CreateMovieCommand CreateMovieCommand(
        string? title = null)
    {
        var faker = new Faker();

        return new CreateMovieCommand(
            title ?? faker.Commerce.ProductName(),
            faker.Lorem.Sentence(),
            faker.Date.Past(20).Year,
            faker.Random.Decimal(1, 20));
    }

    public static Faker<CreateUserCommand> CreateUserCommandFaker()
        => new Faker<CreateUserCommand>()
            .CustomInstantiator(f =>
                new CreateUserCommand
                {
                    UserName = f.Internet.UserName(),
                    Email = f.Internet.Email(),
                    Password = "Password123!"
                });

    // MOVIE PHYSICAL COPY
    public static List<MoviePhysicalCopy> PhysicalCopies(
        Movie movie,
        int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => CreateMovieCopy(i, movie))
            .ToList();
    }

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
    public static User CreateUser(
        string? id = null,
        bool withRentals = false)
    {
        var faker = new Faker();
        var email = faker.Internet.Email();
        var userName = faker.Internet.UserName();


        var user = new User
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


        if (withRentals)
        {
            var movie = CreateMovie();
            var copy = CreateMovieCopy(
                1,
                movie);

            user.Rentals.Add(
                CreateRental(
                    1,
                    user,
                    copy));
        }

        return user;
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


    // ROLE
    public static IdentityRole CreateRole(
        string name = "Admin")
    {
        return new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            NormalizedName = name.ToUpper()
        };
    }
}