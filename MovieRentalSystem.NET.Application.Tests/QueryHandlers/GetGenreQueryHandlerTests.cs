using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetGenreQueryHandlerTests
{
    private static IDbContext CreateDbContext(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();
        var genreDbSet = genres.BuildMockDbSet();
        db.Genres.Returns(genreDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static GetGenreQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetGenreQueryHandler>>());


    [Fact]
    public async Task Handle_ValidPagination_ReturnsPagedGenres()
    {
        // Arrange
        var genres = TestData.Genres(5)
            .OrderBy(x => x.Id)
            .ToList();

        var handler = CreateHandler(CreateDbContext(genres));

        var query = new GetGenreQuery
        {
            PageNumber = 1,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().BeEquivalentTo(
            genres.Take(2).Select(x => x.MapToGenreDto()),
            o => o.WithStrictOrdering());
        result.TotalRecords.Should().Be(5);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyResult()
    {
        // Arrange
        var handler = CreateHandler(CreateDbContext(new List<Genre>()));

        // Act
        var result = await handler.Handle(new GetGenreQuery
        {
            PageNumber = 1,
            PageSize = 10
        }, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
        result.TotalRecords.Should().Be(0);
    }

    [Fact]
    public async Task Handle_PageNumberTwo_ReturnsCorrectPage()
    {
        // Arrange
        var genres = TestData.Genres(10)
            .OrderBy(x => x.Id)
            .ToList();

        var handler = CreateHandler(CreateDbContext(genres));

        // Act
        var result = await handler.Handle(new GetGenreQuery
        {
            PageNumber = 2,
            PageSize = 3
        }, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(3);
        result.TotalRecords.Should().Be(10);
    }
}