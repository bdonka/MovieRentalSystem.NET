using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class CreateMovieCommandHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();

        movieDbSet
            .When(x => x.Add(Arg.Any<Movie>()))
            .Do(call =>
            {
                var movie = call.Arg<Movie>();

                if (movie.Id == 0)
                {
                    movie.Id = movies.Count == 0
                        ? 1
                        : movies.Max(m => m.Id) + 1;
                }

                movies.Add(movie);
            });

        db.Movies.Returns(movieDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static CreateMovieCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<CreateMovieCommandHandler>>());

    [Fact]
    public async Task Handle_ValidMovie_CreatesMovieSuccessfully()
    {
        // Arrange
        var movies = new List<Movie>();

        var db = CreateDb(movies);

        var handler = CreateHandler(db);

        var command = TestData.CreateMovieCommandFaker().Generate();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        movies.Should().ContainSingle(m =>
            m.Title == command.Title &&
            m.Description == command.Description &&
            m.ReleaseYear == command.ReleaseYear &&
            m.RentalPrice == command.RentalPrice);

        result.Value.Title.Should().Be(command.Title);

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ExistingMovieTitle_ReturnsMovieAlreadyExistsError()
    {
        // Arrange
        var existingMovie = TestData.MovieFaker().Generate();
        existingMovie.Title = "The Matrix";

        var movies = new List<Movie> { existingMovie };

        var db = CreateDb(movies);

        var handler = CreateHandler(db);

        var command = TestData.CreateMovieCommandFaker().Generate() with { Title = existingMovie.Title };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should().BeOfType<MovieAlreadyExistsError>();

        movies.Should().HaveCount(1);

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
