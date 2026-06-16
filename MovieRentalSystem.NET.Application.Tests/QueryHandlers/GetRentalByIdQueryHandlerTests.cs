using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetRentalByIdQueryHandlerTests
{
    private static IDbContext CreateDb(List<Rental> rentals)
    {
        var db = Substitute.For<IDbContext>();

        db.Rentals.Returns(rentals.BuildMockDbSet());

        return db;
    }

    private static GetRentalByIdQueryHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<GetRentalByIdQueryHandler>>());

    [Fact]
    public async Task Handle_RentalExists_ReturnsRentalDto()
    {
        // Arrange
        var user = TestData.CreateUser();

        var movie = TestData.CreateMovie();

        var copy = TestData.CreateMovieCopy(
            id: 10,
            movie: movie,
            status: MovieCopyStatus.Rented
        );

        var rental = TestData.CreateRental(
            id: 1,
            user: user,
            copy: copy,
            status: RentalStatus.Active
        );

        var db = CreateDb(new List<Rental> { rental });
        var handler = CreateHandler(db);

        var query = new GetRentalByIdQuery
        {
            Id = 1
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.Id.Should().Be(1);
        result.Value.UserId.Should().Be(user.Id);
        result.Value.MoviePhysicalCopyId.Should().Be(copy.Id);
    }

    [Fact]
    public async Task Handle_RentalDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var db = CreateDb(new List<Rental>());
        var handler = CreateHandler(db);

        var query = new GetRentalByIdQuery
        {
            Id = 999
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RentalNotFoundError>();
    }
}