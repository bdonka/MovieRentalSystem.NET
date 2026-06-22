using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class CreateMoviePhysicalCopyCommandHandlerTests
{
    private static IDbContext CreateDb(List<MoviePhysicalCopy> copies, List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();
        var copyDbSet = copies.BuildMockDbSet();

        db.Movies.Returns(movieDbSet);
        db.MoviePhysicalCopies.Returns(copyDbSet);

        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static CreateMoviePhysicalCopyCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<CreateMoviePhysicalCopyCommandHandler>>());

    [Fact]
    public async Task Handle_ValidRequest_CreatesPhysicalCopySuccessfully()
    {
        // Arrange
        var movie = TestData.CreateMovie(1);

        var movies = new List<Movie> { movie };
        var copies = new List<MoviePhysicalCopy>();

        var db = CreateDb(copies, movies);
        var handler = CreateHandler(db);

        var command = new CreateMoviePhysicalCopyCommand
        {
            MovieId = movie.Id,
            Code = "COPY-001"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        db.MoviePhysicalCopies.Received(1).Add(
            Arg.Is<MoviePhysicalCopy>(c =>
                c.MovieId == movie.Id &&
                c.Code == "COPY-001"));

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DuplicateCode_ReturnsAlreadyExistsError()
    {
        // Arrange
        var movie = TestData.CreateMovie(1);

        var existingCopy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = movie.Id,
            Movie = movie,
            Code = "COPY-001",
            Status = MovieCopyStatus.Available
        };

        var movies = new List<Movie> { movie };
        var copies = new List<MoviePhysicalCopy> { existingCopy };

        var db = CreateDb(copies, movies);
        var handler = CreateHandler(db);

        var command = new CreateMoviePhysicalCopyCommand
        {
            MovieId = movie.Id,
            Code = "COPY-001"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should().BeOfType<MoviePhysicalCopyAlreadyExistsError>();

        copies.Should().HaveCount(1);

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AssignsMovieRelation_WhenCreatingCopy()
    {
        // Arrange
        var movie = TestData.CreateMovie(1);

        var movies = new List<Movie> { movie };
        var copies = new List<MoviePhysicalCopy>();

        var db = CreateDb(copies, movies);
        db.Movies.Should().ContainSingle();
        var handler = CreateHandler(db);

        var command = new CreateMoviePhysicalCopyCommand
        {
            MovieId = movie.Id,
            Code = "COPY-REL"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        db.MoviePhysicalCopies.Received(1).Add(
            Arg.Is<MoviePhysicalCopy>(c =>
                c.MovieId == movie.Id &&
                c.Code == "COPY-REL"));
    }
}