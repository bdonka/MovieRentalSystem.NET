using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Application.Tests.Common;
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
    public async Task Should_Return_Genre_When_Exists()
    {
        var genres = TestData.Genres(5);
        var target = genres.First();

        var handler = CreateHandler(CreateDb(genres));

        var result = await handler.Handle(
            new GetGenreByIdQuery { Id = target.Id },
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(target.Id);
    }

    [Fact]
    public async Task Should_Return_Error_When_Not_Found()
    {
        var handler = CreateHandler(CreateDb(new List<Genre>()));

        var result = await handler.Handle(
            new GetGenreByIdQuery { Id = 999 },
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}