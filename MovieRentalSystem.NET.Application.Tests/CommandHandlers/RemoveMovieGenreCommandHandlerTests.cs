using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class RemoveMovieGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();

        db.Movies.Returns(movieDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static RemoveMovieGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<RemoveMovieGenreCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingMovieAndAssignedGenre_RemovesGenreSuccessfully()
    {
        // Arrange
        var genres = TestData.GenreFaker().Generate(3);
        var genreToRemove = genres[0];

        var movie = new Movie
        {
            Id = 1,
            Genres = genres
        };

        var db = CreateDb(new List<Movie> { movie });

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new RemoveMovieGenreCommand
        {
            MovieId = 1,
            GenreId = genreToRemove.Id
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        movie.Genres.Should().HaveCount(2);
        movie.Genres.Should().NotContain(g => g.Id == genreToRemove.Id);
        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MissingMovie_ReturnsMovieNotFoundError()
    {
        // Arrange
        var genre = TestData.GenreFaker().Generate();

        var db = CreateDb(
            new List<Movie>());

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new RemoveMovieGenreCommand
        {
            MovieId = 1,
            GenreId = genre.Id
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should().ContainSingle().Which.Should().BeOfType<MovieNotFoundError>();

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MovieWithoutGenreAssignment_ReturnsSuccessWithoutChanges()
    {
        // Arrange
        var genres = TestData.GenreFaker().Generate(3);
        var notExistingGenreId = genres.MaxBy(g => g.Id)!.Id + 1;

        var movie = new Movie
        {
            Id = 1,
            Genres = genres
        };

        var db = CreateDb(new List<Movie> { movie });

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new RemoveMovieGenreCommand
        {
            MovieId = 1,
            GenreId = notExistingGenreId
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        movie.Genres.Should().BeEquivalentTo(genres);

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}