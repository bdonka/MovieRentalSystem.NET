using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class UpdateRentalCommandHandlerTests
{
    private static IDbContext CreateDb(
        List<User> users,
        List<Rental> rentals,
        List<MoviePhysicalCopy> copies)
    {
        var db = Substitute.For<IDbContext>();

        var rentalsDbSet = rentals.BuildMockDbSet();
        var copiesDbSet = copies.BuildMockDbSet();

        db.Rentals.Returns(rentalsDbSet);
        db.MoviePhysicalCopies.Returns(copiesDbSet);

        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static UpdateRentalCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<UpdateRentalCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingRental_UpdatesRentalSuccessfully()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "testuser",
            Email = "test@test.com"
        };

        var rental = new Rental
        {
            Id = 1,
            UserId = user.Id,
            MoviePhysicalCopyId = 10,
            RentalStartDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(5),
            Status = RentalStatus.Active
        };

        var rentals = new List<Rental> { rental };
        var copies = new List<MoviePhysicalCopy>();

        var db = CreateDb(new List<User>(), rentals, copies);
        var handler = CreateHandler(db);

        var command = new UpdateRentalCommand
        {
            Id = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(10),
            ReturnDate = null,
            TotalPrice = 99,
            Status = RentalStatus.Active
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        rental.TotalPrice.Should().Be(99);
        rental.DueDate.Should().Be(command.DueDate);

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RentalDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var db = CreateDb(
            new List<User>(),
            new List<Rental>(),
            new List<MoviePhysicalCopy>());

        var handler = CreateHandler(db);

        var command = new UpdateRentalCommand
        {
            Id = 999,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(10),
            ReturnDate = null,
            TotalPrice = 50,
            Status = RentalStatus.Active
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RentalNotFoundError>();

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithReturnDate_UpdatesCopyStatusToAvailable()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "testuser",
            Email = "test@test.com"
        };

        var rental = new Rental
        {
            Id = 1,
            UserId = user.Id,
            MoviePhysicalCopyId = 10,
            Status = RentalStatus.Active
        };

        var copy = new MoviePhysicalCopy
        {
            Id = 10,
            Status = MovieCopyStatus.Rented
        };

        var db = CreateDb(
            new List<User> { user },
            new List<Rental> { rental },
            new List<MoviePhysicalCopy> { copy });

        var handler = CreateHandler(db);

        var command = new UpdateRentalCommand
        {
            Id = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(10),
            ReturnDate = DateTime.UtcNow,
            TotalPrice = 100,
            Status = RentalStatus.Completed
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        copy.Status.Should().Be(MovieCopyStatus.Available);

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}