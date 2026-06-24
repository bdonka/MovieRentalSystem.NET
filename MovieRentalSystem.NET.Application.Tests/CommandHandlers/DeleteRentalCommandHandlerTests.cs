using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class DeleteRentalCommandHandlerTests
{
    private static IDbContext CreateDb(List<Rental> rentals)
    {
        var db = Substitute.For<IDbContext>();

        var rentalsDbSet = rentals.BuildMockDbSet();

        db.Rentals.Returns(rentalsDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static DeleteRentalCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<DeleteRentalCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingRental_DeletesSuccessfully()
    {
        // Arrange
        var movie = TestData.CreateMovie();

        var rental = TestData.CreateRental(
            id: 1,
            user: TestData.CreateUser(),
            copy: TestData.CreateMovieCopy(1, movie));

        var rentals = new List<Rental> { rental };

        var db = CreateDb(rentals);
        var handler = CreateHandler(db);

        var command = new DeleteRentalCommand
        {
            Id = rental.Id
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        db.Rentals.Received(1)
            .Remove(Arg.Is<Rental>(r => r.Id == rental.Id));

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RentalDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var rentals = new List<Rental>();

        var db = CreateDb(rentals);
        var handler = CreateHandler(db);

        var command = new DeleteRentalCommand
        {
            Id = 999
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RentalNotFoundError>();

        db.Rentals.DidNotReceive()
            .Remove(Arg.Any<Rental>());

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}