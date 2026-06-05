using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Application.Tests.Common;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetGenreQueryHandlerTests
{
    private static IDbContext CreateDbContext(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();
        db.Genres.Returns(genres.BuildMock());
        return db;
    }

    private static GetGenreQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetGenreQueryHandler>>());

    [Fact]
    public async Task Should_Return_Paged_Genres()
    {
        var genres = TestData.Genres(5);

        var handler = CreateHandler(CreateDbContext(genres));

        var query = new GetGenreQuery
        {
            PageNumber = 1,
            PageSize = 2
        };

        var result = await handler.Handle(query, CancellationToken.None);

        result.Data.Should().HaveCount(2);
        result.TotalRecords.Should().Be(5);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task Should_Return_Empty_When_No_Data()
    {
        var handler = CreateHandler(CreateDbContext(new List<Genre>()));

        var result = await handler.Handle(new GetGenreQuery
        {
            PageNumber = 1,
            PageSize = 10
        }, CancellationToken.None);

        result.Data.Should().BeEmpty();
        result.TotalRecords.Should().Be(0);
    }

    [Fact]
    public async Task Should_Apply_Pagination()
    {
        var genres = TestData.Genres(10);

        var handler = CreateHandler(CreateDbContext(genres));

        var result = await handler.Handle(new GetGenreQuery
        {
            PageNumber = 2,
            PageSize = 3
        }, CancellationToken.None);

        result.Data.Should().HaveCount(3);
        result.TotalRecords.Should().Be(10);
    }
}