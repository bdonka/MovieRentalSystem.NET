using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class UpdateUserCommandHandlerTests
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


    private static UpdateUserCommandHandler CreateHandler(
        UserManager<User> userManager)
        => new(
            userManager,
            Substitute.For<ILogger<UpdateUserCommandHandler>>());


    [Fact]
    public async Task Handle_ExistingUserWithAvailableEmail_ReturnsSuccess()
    {
        // Arrange
        var user = TestData.CreateUser();

        var command = new UpdateUserCommand
        {
            Id = user.Id,
            UserName = "new_username",
            Email = "newemail@test.com"
        };

        var userManager = CreateUserManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        userManager.FindByEmailAsync(command.Email)
            .Returns((User?)null);

        userManager.UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var handler = CreateHandler(userManager);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();

        user.UserName.Should()
            .Be(command.UserName);

        user.Email.Should()
            .Be(command.Email);

        await userManager.Received(1)
            .UpdateAsync(user);
    }


    [Fact]
    public async Task Handle_MissingUser_ReturnsUserNotFoundError()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = "missing-id",
            UserName = "new_username",
            Email = "email@test.com"
        };

        var userManager = CreateUserManager();

        userManager.FindByIdAsync(command.Id)
            .Returns((User?)null);

        var handler = CreateHandler(userManager);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserNotFoundError>();

        await userManager.DidNotReceive()
            .UpdateAsync(Arg.Any<User>());
    }


    [Fact]
    public async Task Handle_EmailAlreadyUsedByAnotherUser_ReturnsUserAlreadyExistsError()
    {
        // Arrange
        var user = TestData.CreateUser();

        var anotherUser = TestData.CreateUser();

        var command = new UpdateUserCommand
        {
            Id = user.Id,
            UserName = "new_username",
            Email = anotherUser.Email!
        };

        var userManager = CreateUserManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        userManager.FindByEmailAsync(command.Email)
            .Returns(anotherUser);

        var handler = CreateHandler(userManager);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserAlreadyExistsError>();

        await userManager.DidNotReceive()
            .UpdateAsync(Arg.Any<User>());
    }


    [Fact]
    public async Task Handle_UserUpdatesOwnEmail_ReturnsSuccess()
    {
        // Arrange
        var user = TestData.CreateUser();

        var command = new UpdateUserCommand
        {
            Id = user.Id,
            UserName = "changed_username",
            Email = user.Email!
        };

        var userManager = CreateUserManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        userManager.FindByEmailAsync(command.Email)
            .Returns(user);

        userManager.UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var handler = CreateHandler(userManager);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();

        await userManager.Received(1)
            .UpdateAsync(user);
    }
}