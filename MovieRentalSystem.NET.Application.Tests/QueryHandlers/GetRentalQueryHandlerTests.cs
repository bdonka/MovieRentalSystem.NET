using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetRentalQueryHandlerTests
{
    private static IDbContext CreateDb(List<Rental> rentals)
    {
        var db = Substitute.For<IDbContext>();

        db.Rentals.Returns(rentals.BuildMockDbSet());

        return db;
    }

    private static GetRentalQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetRentalQueryHandler>>());

    [Fact]
    public async Task Handle_ReturnsPagedRentals_CorrectPageAndTotal()
    {
        // Arrange
        var user = TestData.CreateUser();

        var movie = TestData.CreateMovie();
        var copy = TestData.CreateMovieCopy(1, movie);

        var rentals = Enumerable.Range(1, 5)
            .Select(i => TestData.CreateRental(
                id: i,
                user: user,
                copy: copy))
            .ToList();

        var db = CreateDb(rentals);
        var handler = CreateHandler(db);

        var query = new GetRentalQuery
        {
            PageNumber = 1,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.TotalRecords.Should().Be(5);
    }

    [Fact]
    public async Task Handle_EmptyDb_ReturnsEmptyPagedResponse()
    {
        // Arrange
        var db = CreateDb(new List<Rental>());
        var handler = CreateHandler(db);

        var query = new GetRentalQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
        result.TotalRecords.Should().Be(0);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}