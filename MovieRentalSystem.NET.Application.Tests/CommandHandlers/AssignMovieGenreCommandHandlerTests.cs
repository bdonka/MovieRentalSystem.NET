using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class AssignMovieGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies, List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();
        var genreDbSet = genres.BuildMockDbSet();

        db.Movies.Returns(movieDbSet);
        db.Genres.Returns(genreDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static AssignMovieGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<AssignMovieGenreCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingMovieAndGenre_ReturnsSuccess()
    {
        // Arrange
        var genre = TestData.GenreFaker().Generate();

        var movie = new Movie { Id = 1, Genres = new List<Genre>() };

        var db = CreateDb(
            new List<Movie> { movie },
            new List<Genre> { genre });

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new AssignMovieGenreCommand
        {
            MovieId = 1,
            GenreId = genre.Id
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        movie.Genres.Should().ContainSingle(g => g.Id == genre.Id);
    }

    [Fact]
    public async Task Handle_MissingMovie_ReturnsMovieNotFoundError()
    {
        // Arrange
        var genre = TestData.GenreFaker().Generate();

        var db = CreateDb(
            new List<Movie>(),
            new List<Genre> { genre });

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new AssignMovieGenreCommand
        {
            MovieId = 1,
            GenreId = genre.Id
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<MovieNotFoundError>();
    }

    [Fact]
    public async Task Handle_MissingGenre_ReturnsGenreNotFoundError()
    {
        // Arrange
        var movie = new Movie { Id = 1, Genres = new List<Genre>() };

        var db = CreateDb(
            new List<Movie> { movie },
            new List<Genre>());

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new AssignMovieGenreCommand
        {
            MovieId = 1,
            GenreId = 999
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<GenreNotFoundError>();
    }

    [Fact]
    public async Task Handle_GenreAlreadyAssigned_ReturnsGenreAlreadyAssignedError()
    {
        // Arrange
        var genre = TestData.GenreFaker().Generate();

        var movie = new Movie
        {
            Id = 1,
            Genres = new List<Genre> { genre }
        };

        var db = CreateDb(
            new List<Movie> { movie },
            new List<Genre> { genre });

        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new AssignMovieGenreCommand
        {
            MovieId = 1,
            GenreId = genre.Id
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<GenreAlreadyAssignedToMovieError>();
    }
}