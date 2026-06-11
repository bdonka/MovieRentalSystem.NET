using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class UpdateMovieCommandHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();

        db.Movies.Returns(movieDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static UpdateMovieCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<UpdateMovieCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingMovie_UpdatesMovieSuccessfully()
    {
        // Arrange
        var movie = TestData.CreateMovie();
        movie.Id = 1;
        movie.Title = "Old Title";
        movie.Description = "Old Desc";
        movie.ReleaseYear = 2000;
        movie.RentalPrice = 5m;

        var movies = new List<Movie> { movie };

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var command = new UpdateMovieCommand(
            1,
            "New Title",
            "New Desc",
            2024,
            15m);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        movie.Title.Should().Be("New Title");
        movie.Description.Should().Be("New Desc");
        movie.ReleaseYear.Should().Be(2024);
        movie.RentalPrice.Should().Be(15m);

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MovieNotFound_ReturnsMovieNotFoundError()
    {
        // Arrange
        var movies = new List<Movie>();

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var command = new UpdateMovieCommand(
            999,
            "Title",
            "Desc",
            2024,
            10m);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should().BeOfType<MovieNotFoundError>();

        movies.Should().BeEmpty();

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DuplicateTitle_ReturnsMovieAlreadyExistsError()
    {
        // Arrange
        var existingMovie = TestData.CreateMovie();
        existingMovie.Id = 1;
        existingMovie.Title = "Matrix";
        existingMovie.Description = "Old";
        existingMovie.ReleaseYear = 2000;
        existingMovie.RentalPrice = 10m;

        var otherMovie = TestData.CreateMovie();
        otherMovie.Id = 2;
        otherMovie.Title = "Other";
        otherMovie.Description = "X";
        otherMovie.ReleaseYear = 2010;
        otherMovie.RentalPrice = 5m;

        var movies = new List<Movie>
        {
            existingMovie,
            otherMovie
        };

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var command = new UpdateMovieCommand(
            2,
            "Matrix",
            "New Desc",
            2024,
            20m);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should().BeOfType<MovieAlreadyExistsError>();

        movies.Should().ContainSingle(m => m.Id == 2);

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}