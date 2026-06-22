using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetMovieByIdQueryHandlerTests
{
    private static IDbContext CreateDb(List<Movie> movies)
    {
        var db = Substitute.For<IDbContext>();

        var movieDbSet = movies.BuildMockDbSet();

        db.Movies.Returns(movieDbSet);

        return db;
    }

    private static GetMovieByIdQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetMovieByIdQueryHandler>>());

    [Fact]
    public async Task Handle_ExistingMovie_ReturnsMovieDto()
    {
        // Arrange
        var movie = TestData.CreateMovie();

        movie.Title = "Matrix";
        movie.Description = "Sci-fi";
        movie.ReleaseYear = 1999;
        movie.RentalPrice = 10m;

        var db = CreateDb(new List<Movie> { movie });
        var handler = CreateHandler(db);

        var query = new GetMovieByIdQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Matrix");
        result.Value.ReleaseYear.Should().Be(1999);
    }

    [Fact]
    public async Task Handle_MovieNotFound_ReturnsMovieNotFoundError()
    {
        // Arrange
        var db = CreateDb(new List<Movie>());
        var handler = CreateHandler(db);

        var query = new GetMovieByIdQuery(999);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should().BeOfType<MovieNotFoundError>();
    }
}