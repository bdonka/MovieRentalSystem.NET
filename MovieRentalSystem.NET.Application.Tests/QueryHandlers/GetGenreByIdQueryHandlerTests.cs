using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetGenreByIdQueryHandlerTests
{
    private static IDbContext CreateDb(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();
        db.Genres.Returns(genres.BuildMock());
        return db;
    }

    private static GetGenreByIdQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetGenreByIdQueryHandler>>());

    [Fact]
    public async Task Handle_ExistingGenreId_ReturnsGenreDto()
    {
        // Arrange
        var genres = TestData.Genres(5);
        var target = genres.First();

        var handler = CreateHandler(CreateDb(genres));

        // Act
        var result = await handler.Handle(
            new GetGenreByIdQuery { Id = target.Id },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(target.MapToGenreDto());
    }

    [Fact]
    public async Task Handle_NonExistingGenreId_ReturnsGenreNotFoundError()
    {
        // Arrange
        var handler = CreateHandler(CreateDb(new List<Genre>()));

        // Act
        var result = await handler.Handle(
            new GetGenreByIdQuery { Id = 999 },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e is GenreNotFoundError);
    }
}