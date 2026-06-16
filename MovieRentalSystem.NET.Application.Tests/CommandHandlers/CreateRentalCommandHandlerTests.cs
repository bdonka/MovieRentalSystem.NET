using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class CreateRentalCommandHandlerTests
{
    private static IDbContext CreateDb(
        List<User> users,
        List<MoviePhysicalCopy> copies,
        List<Rental> rentals)
    {
        var db = Substitute.For<IDbContext>();

        var userDbSet = users.BuildMockDbSet();
        var copyDbSet = copies.BuildMockDbSet();
        var rentalDbSet = rentals.BuildMockDbSet();

        db.Users.Returns(userDbSet);
        db.MoviePhysicalCopies.Returns(copies.BuildMockDbSet());
        db.Rentals.Returns(rentals.BuildMockDbSet());

        db.SaveChangesAsync(default)
            .ReturnsForAnyArgs(1);

        return db;
    }

    private static CreateRentalCommandHandler CreateHandler(IDbContext db)
        => new(
            db,
            Substitute.For<ILogger<CreateRentalCommandHandler>>());

    [Fact]
    public async Task Handle_ValidRequest_CreatesRentalSuccessfully()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            DateRegistered = faker.Date.Past()
        };

        var copy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = faker.Random.Int(1, 100),
            Code = $"COPY-{faker.Random.AlphaNumeric(6)}"
        };

        var users = new List<User> { user };
        var copies = new List<MoviePhysicalCopy> { copy };
        var rentals = new List<Rental>();

        var db = CreateDb(users, copies, rentals);
        var handler = CreateHandler(db);

        var command = new CreateRentalCommand
        {
            UserId = user.Id,
            MoviePhysicalCopyId = copy.Id,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        db.Rentals.Received(1).Add(
            Arg.Is<Rental>(r =>
                r.UserId == command.UserId &&
                r.MoviePhysicalCopyId == command.MoviePhysicalCopyId &&
                r.RentalStartDate == command.RentalStartDate &&
                r.DueDate == command.DueDate));

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsUserNotFoundError()
    {
        // Arrange
        var faker = new Faker();

        var users = new List<User>();
        var copies = new List<MoviePhysicalCopy>();
        var rentals = new List<Rental>();

        var db = CreateDb(users, copies, rentals);
        var handler = CreateHandler(db);

        var command = new CreateRentalCommand
        {
            UserId = Guid.NewGuid().ToString(),
            MoviePhysicalCopyId = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserNotFoundError>();

        db.Rentals.DidNotReceive()
            .Add(Arg.Any<Rental>());

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MoviePhysicalCopyDoesNotExist_ReturnsMoviePhysicalCopyNotFoundError()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            DateRegistered = faker.Date.Past()
        };

        var users = new List<User> { user };
        var copies = new List<MoviePhysicalCopy>();
        var rentals = new List<Rental>();

        var db = CreateDb(users, copies, rentals);
        var handler = CreateHandler(db);

        var command = new CreateRentalCommand
        {
            UserId = Guid.NewGuid().ToString(),
            MoviePhysicalCopyId = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<MoviePhysicalCopyNotFoundError>();

        db.Rentals.DidNotReceive()
            .Add(Arg.Any<Rental>());

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}