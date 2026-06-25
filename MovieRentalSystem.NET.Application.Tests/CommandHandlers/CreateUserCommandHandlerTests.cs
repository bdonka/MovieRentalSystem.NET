using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class CreateUserCommandHandlerTests
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

    private static CreateUserCommandHandler CreateHandler(
        UserManager<User> userManager)
        => new(
            userManager,
            Substitute.For<ILogger<CreateUserCommandHandler>>());


    [Fact]
    public async Task Handle_NewUser_ReturnsCreatedUser()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            UserName = "john",
            Email = "john@test.com",
            Password = "Password123!"
        };

        var userManager = CreateUserManager();

        userManager.FindByEmailAsync(command.Email)
            .Returns((User?) null);

        userManager.CreateAsync(
                Arg.Any<User>(),
                command.Password)
            .Returns(IdentityResult.Success);

        var handler = CreateHandler(userManager);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be(command.Email);
        result.Value.UserName.Should().Be(command.UserName);

        await userManager.Received(1)
            .CreateAsync(
                Arg.Is<User>(u =>
                    u.Email == command.Email &&
                    u.UserName == command.UserName),
                command.Password);
    }


    [Fact]
    public async Task Handle_UserAlreadyExists_ReturnsUserAlreadyExistsError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            UserName = "john",
            Email = "john@test.com",
            Password = "Password123!"
        };

        var existingUser = TestData.CreateUser();

        var userManager = CreateUserManager();

        userManager.FindByEmailAsync(command.Email)
            .Returns(existingUser);

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
            .CreateAsync(
                Arg.Any<User>(),
                Arg.Any<string>());
    }


    [Fact]
    public async Task Handle_CreateAsyncFails_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            UserName = "john",
            Email = "john@test.com",
            Password = "Password123!"
        };

        var userManager = CreateUserManager();

        userManager.FindByEmailAsync(command.Email)
            .Returns((User?) null);

        userManager.CreateAsync(
                Arg.Any<User>(),
                command.Password)
            .Returns(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Password too weak"
                    }));

        var handler = CreateHandler(userManager);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}