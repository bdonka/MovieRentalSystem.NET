using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Application.Tests.Common;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class AssignMovieGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies, List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();
        db.Movies.Returns(movies.BuildMock());
        db.Genres.Returns(genres.BuildMock());
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static AssignMovieGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<AssignMovieGenreCommandHandler>>());

    [Fact]
    public async Task Should_Assign_Genre()
    {
        var genre = TestData.GenreFaker().Generate();

        var movie = new Movie { Id = 1, Genres = new List<Genre>() };

        var db = CreateDb(
            new List<Movie> { movie },
            new List<Genre> { genre });

        var handler = CreateHandler(db);

        var result = await handler.Handle(new AssignMovieGenreCommand
        {
            MovieId = 1,
            GenreId = genre.Id
        }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        movie.Genres.Should().ContainSingle();
    }
}