using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetMovieQueryHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();

        db.Movies.Returns(movieDbSet);

        return db;
    }

    private static GetMovieQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetMovieQueryHandler>>());

    [Fact]
    public async Task Handle_ReturnsPagedMovies_CorrectPage()
    {
        // Arrange
        var movies = new List<Movie>
        {
            TestData.CreateMovie(1),
            TestData.CreateMovie(2),
            TestData.CreateMovie(3),
            TestData.CreateMovie(4)
        };

        movies[0].Title = "A";
        movies[1].Title = "B";
        movies[2].Title = "C";
        movies[3].Title = "D";

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var query = new GetMovieQuery
        {
            PageNumber = 1,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(2);
        result.TotalRecords.Should().Be(4);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);

        result.Data.Select(x => x.Title)
            .Should()
            .BeEquivalentTo(new[] { "A", "B" });
    }

    [Fact]
    public async Task Handle_SecondPage_ReturnsCorrectItems()
    {
        // Arrange
        var movies = new List<Movie>
        {
            TestData.CreateMovie(1),
            TestData.CreateMovie(2),
            TestData.CreateMovie(3),
            TestData.CreateMovie(4)
        };

        movies[0].Title = "A";
        movies[1].Title = "B";
        movies[2].Title = "C";
        movies[3].Title = "D";

        var db = CreateDb(movies);
        var handler = CreateHandler(db);

        var query = new GetMovieQuery
        {
            PageNumber = 2,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(2);
        result.TotalRecords.Should().Be(4);

        result.Data.Select(x => x.Title)
            .Should()
            .BeEquivalentTo(new[] { "C", "D" });
    }

    [Fact]
    public async Task Handle_NoMovies_ReturnsEmptyPage()
    {
        // Arrange
        var db = CreateDb(new List<Movie>());
        var handler = CreateHandler(db);

        var query = new GetMovieQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
        result.TotalRecords.Should().Be(0);
    }
}