using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class DeleteUserCommandHandlerTests
{
    private static UserManager<User> CreateUserManager()
    {
        return Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
    }

    private static IDbContext CreateDbContext(
        List<Rental> rentals)
    {
        var db = Substitute.For<IDbContext>();
        var rentalDbSet = rentals.BuildMockDbSet();

        db.Rentals.Returns(rentalDbSet);

        return db;
    }


    private static DeleteUserCommandHandler CreateHandler(
        UserManager<User> userManager,
        IDbContext dbContext)
        => new(
            userManager,
            dbContext,
            Substitute.For<ILogger<DeleteUserCommandHandler>>());


    [Fact]
    public async Task Handle_UserWithoutRentals_ReturnsSuccess()
    {
        // Arrange
        var user = TestData.CreateUser();

        var userManager = CreateUserManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        userManager.DeleteAsync(user)
            .Returns(IdentityResult.Success);

        var db = CreateDbContext(
            new List<Rental>());

        var handler = CreateHandler(
            userManager,
            db);


        // Act
        var result = await handler.Handle(
            new DeleteUserCommand
            {
                Id = user.Id
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();

        await userManager.Received(1)
            .DeleteAsync(user);
    }


    [Fact]
    public async Task Handle_MissingUser_ReturnsUserNotFoundError()
    {
        // Arrange
        var userManager = CreateUserManager();

        userManager.FindByIdAsync("missing-id")
            .Returns((User?) null);

        var db = CreateDbContext(
            new List<Rental>());

        var handler = CreateHandler(
            userManager,
            db);


        // Act
        var result = await handler.Handle(
            new DeleteUserCommand
            {
                Id = "missing-id"
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserNotFoundError>();

        await userManager.DidNotReceive()
            .DeleteAsync(Arg.Any<User>());
    }


    [Fact]
    public async Task Handle_UserHasRentals_ReturnsUserHasAssignedRentalsError()
    {
        // Arrange
        var user = TestData.CreateUser();
        var movie = TestData.CreateMovie();
        var copy = TestData.CreateMovieCopy(1, movie);
        var rental = TestData.CreateRental(1, user, copy);
        var userManager = CreateUserManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        var db = CreateDbContext(
            new List<Rental>
            {
                rental
            });


        var handler = CreateHandler(
            userManager,
            db);

        // Act
        var result = await handler.Handle(
            new DeleteUserCommand
            {
                Id = user.Id
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserHasAssignedRentalsError>();

        await userManager.DidNotReceive()
            .DeleteAsync(user);
    }
}