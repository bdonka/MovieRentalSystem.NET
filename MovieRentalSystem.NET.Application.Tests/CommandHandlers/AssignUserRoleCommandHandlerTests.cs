using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class AssignUserRoleCommandHandlerTests
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

    private static RoleManager<IdentityRole> CreateRoleManager()
    {
        return Substitute.For<RoleManager<IdentityRole>>(
            Substitute.For<IRoleStore<IdentityRole>>(),
            null!,
            null!,
            null!,
            null!);
    }

    private static AssignUserRoleCommandHandler CreateHandler(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
        => new(
            userManager,
            roleManager,
            Substitute.For<ILogger<AssignUserRoleCommandHandler>>());


    [Fact]
    public async Task Handle_ExistingUserAndRole_ReturnsSuccess()
    {
        // Arrange
        var user = TestData.CreateUser();
        var role = TestData.CreateRole();

        var userManager = CreateUserManager();
        var roleManager = CreateRoleManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        roleManager.RoleExistsAsync(role.Name!)
            .Returns(true);

        userManager.GetRolesAsync(user)
            .Returns(new List<string>());

        userManager.AddToRoleAsync(user, role.Name!)
            .Returns(IdentityResult.Success);

        var handler = CreateHandler(userManager, roleManager);


        // Act
        var result = await handler.Handle(
            new AssignUserRoleCommand
            {
                Id = user.Id,
                Role = role.Name!
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();

        await userManager.Received(1)
            .AddToRoleAsync(user, role.Name!);
    }


    [Fact]
    public async Task Handle_MissingUser_ReturnsUserNotFoundError()
    {
        // Arrange
        var role = TestData.CreateRole();

        var userManager = CreateUserManager();
        var roleManager = CreateRoleManager();

        userManager.FindByIdAsync("wrong-id")
            .Returns((User?)null);

        var handler = CreateHandler(userManager, roleManager);


        // Act
        var result = await handler.Handle(
            new AssignUserRoleCommand
            {
                Id = "wrong-id",
                Role = role.Name!
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserNotFoundError>();
    }


    [Fact]
    public async Task Handle_MissingRole_ReturnsRoleNotFoundError()
    {
        // Arrange
        var user = TestData.CreateUser();
        var role = TestData.CreateRole();

        var userManager = CreateUserManager();
        var roleManager = CreateRoleManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        roleManager.RoleExistsAsync(role.Name!)
            .Returns(false);

        var handler = CreateHandler(userManager, roleManager);


        // Act
        var result = await handler.Handle(
            new AssignUserRoleCommand
            {
                Id = user.Id,
                Role = role.Name!
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RoleNotFoundError>();
    }


    [Fact]
    public async Task Handle_UserAlreadyHasRole_ReturnsUserAlreadyHasRoleError()
    {
        // Arrange
        var user = TestData.CreateUser();
        var role = TestData.CreateRole();

        var userManager = CreateUserManager();
        var roleManager = CreateRoleManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        roleManager.RoleExistsAsync(role.Name!)
            .Returns(true);

        userManager.GetRolesAsync(user)
            .Returns(new List<string>
            {
                role.Name!
            });

        var handler = CreateHandler(userManager, roleManager);


        // Act
        var result = await handler.Handle(
            new AssignUserRoleCommand
            {
                Id = user.Id,
                Role = role.Name!
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<UserAlreadyHasRoleError>();
    }


    [Fact]
    public async Task Handle_AddRoleFails_ReturnsRoleNotAssignToUserError()
    {
        // Arrange
        var user = TestData.CreateUser();
        var role = TestData.CreateRole();

        var userManager = CreateUserManager();
        var roleManager = CreateRoleManager();

        userManager.FindByIdAsync(user.Id)
            .Returns(user);

        roleManager.RoleExistsAsync(role.Name!)
            .Returns(true);

        userManager.GetRolesAsync(user)
            .Returns(new List<string>());

        userManager.AddToRoleAsync(user, role.Name!)
            .Returns(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Cannot assign role"
                    }));

        var handler = CreateHandler(userManager, roleManager);


        // Act
        var result = await handler.Handle(
            new AssignUserRoleCommand
            {
                Id = user.Id,
                Role = role.Name!
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RoleNotAssignToUserError>();
    }
}