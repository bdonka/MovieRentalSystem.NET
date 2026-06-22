using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class DeleteMovieCommandHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();

        movieDbSet
            .When(x => x.Remove(Arg.Any<Movie>()))
            .Do(call =>
            {
                var entity = call.Arg<Movie>();

                var existing = movies.SingleOrDefault(m => m.Id == entity.Id);
                if (existing != null)
                    movies.Remove(existing);
            });

        db.Movies.Returns(movieDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static DeleteMovieCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<DeleteMovieCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingMovie_DeletesMovieSuccessfully()
    {
        // Arrange
        var movie = TestData.CreateMovie();
        movie.Id = 1;
        movie.Title = "The Matrix";

        var movies = new List<Movie> { movie };

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var command = new DeleteMovieCommand(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        movies.Should().BeEmpty();

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistingMovie_ReturnsMovieNotFoundError()
    {
        // Arrange
        var movies = new List<Movie>();

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var command = new DeleteMovieCommand(999);

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
}