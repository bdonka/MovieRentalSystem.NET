using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetMoviePhysicalCopyByIdQueryHandlerTests
{
    private static IDbContext CreateDb(List<MoviePhysicalCopy> copies)
    {
        var db = Substitute.For<IDbContext>();

        var copyDbSet = copies.BuildMockDbSet();
        db.MoviePhysicalCopies.Returns(copyDbSet);

        return db;
    }

    private static GetMoviePhysicalCopyByIdQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetMoviePhysicalCopyByIdQueryHandler>>());

    [Fact]
    public async Task Handle_ExistingCopy_ReturnsCopyDto()
    {
        // Arrange
        var movie = new Movie
        {
            Id = 1,
            Title = "Matrix"
        };

        var copy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = movie.Id,
            Movie = movie,
            Code = "COPY-001",
            Status = MovieCopyStatus.Available
        };

        var copies = new List<MoviePhysicalCopy> { copy };

        var db = CreateDb(copies);
        var handler = CreateHandler(db);

        var query = new GetMoviePhysicalCopyByIdQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.Id.Should().Be(1);
        result.Value.Code.Should().Be("COPY-001");
        result.Value.MovieId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_CopyNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var db = CreateDb(new List<MoviePhysicalCopy>());
        var handler = CreateHandler(db);

        var query = new GetMoviePhysicalCopyByIdQuery(999);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<MoviePhysicalCopyNotFoundError>();
    }
}