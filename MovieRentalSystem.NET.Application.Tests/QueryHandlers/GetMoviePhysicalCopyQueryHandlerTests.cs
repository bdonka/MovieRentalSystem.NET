using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetMoviePhysicalCopyQueryHandlerTests
{
    private static IDbContext CreateDb(List<MoviePhysicalCopy> copies)
    {
        var db = Substitute.For<IDbContext>();

        var copyDbSet = copies.BuildMockDbSet();
        db.MoviePhysicalCopies.Returns(copyDbSet);

        return db;
    }

    private static GetMoviePhysicalCopyQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetMoviePhysicalCopyQueryHandler>>());

    [Fact]
    public async Task Handle_ReturnsPagedCopies_CorrectPage()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Matrix" };

        var copies = new List<MoviePhysicalCopy>
        {
            new() { Id = 1, MovieId = 1, Movie = movie, Code = "C1", Status = MovieCopyStatus.Available },
            new() { Id = 2, MovieId = 1, Movie = movie, Code = "C2", Status = MovieCopyStatus.Available },
            new() { Id = 3, MovieId = 1, Movie = movie, Code = "C3", Status = MovieCopyStatus.Available },
            new() { Id = 4, MovieId = 1, Movie = movie, Code = "C4", Status = MovieCopyStatus.Available }
        };

        var db = CreateDb(copies);
        var handler = CreateHandler(db);

        var query = new GetMoviePhysicalCopyQuery
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

        result.Data.Select(x => x.Code)
            .Should()
            .BeEquivalentTo(new[] { "C1", "C2" });
    }

    [Fact]
    public async Task Handle_SecondPage_ReturnsCorrectItems()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Matrix" };

        var copies = new List<MoviePhysicalCopy>
        {
            new() { Id = 1, MovieId = 1, Movie = movie, Code = "C1", Status = MovieCopyStatus.Available },
            new() { Id = 2, MovieId = 1, Movie = movie, Code = "C2", Status = MovieCopyStatus.Available },
            new() { Id = 3, MovieId = 1, Movie = movie, Code = "C3", Status = MovieCopyStatus.Available },
            new() { Id = 4, MovieId = 1, Movie = movie, Code = "C4", Status = MovieCopyStatus.Available }
        };

        var db = CreateDb(copies);
        var handler = CreateHandler(db);

        var query = new GetMoviePhysicalCopyQuery
        {
            PageNumber = 2,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(2);
        result.TotalRecords.Should().Be(4);

        result.Data.Select(x => x.Code)
            .Should()
            .BeEquivalentTo(new[] { "C3", "C4" });
    }

    [Fact]
    public async Task Handle_NoCopies_ReturnsEmptyPage()
    {
        // Arrange
        var db = CreateDb(new List<MoviePhysicalCopy>());
        var handler = CreateHandler(db);

        var query = new GetMoviePhysicalCopyQuery
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