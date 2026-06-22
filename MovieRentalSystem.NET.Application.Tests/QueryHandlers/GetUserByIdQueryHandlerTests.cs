using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetUserByIdQueryHandlerTests
{
    private static IDbContext CreateDbContext(
        List<User> users)
    {
        var db = Substitute.For<IDbContext>();
        var userDbSet = users.BuildMockDbSet();

        db.Users.Returns(
            userDbSet);

        return db;
    }


    private static GetUserByIdQueryHandler CreateHandler(
        IDbContext dbContext)
        => new(
            dbContext,
            Substitute.For<ILogger<GetUserByIdQueryHandler>>());



    [Fact]
    public async Task Handle_ExistingUser_ReturnsUserDto()
    {
        // Arrange
        var user = TestData.CreateUser();

        var db = CreateDbContext(
            new List<User>
            {
                user
            });

        var handler = CreateHandler(db);


        // Act
        var result = await handler.Handle(
            new GetUserByIdQuery
            {
                Id = user.Id
            },
            CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();

        result.Value.Id.Should()
            .Be(user.Id);

        result.Value.Email.Should()
            .Be(user.Email);

        result.Value.UserName.Should()
            .Be(user.UserName);
    }



    [Fact]
    public async Task Handle_MissingUser_ReturnsUserNotFoundError()
    {
        // Arrange
        var db = CreateDbContext(
            new List<User>());

        var handler = CreateHandler(db);


        // Act
        var result = await handler.Handle(
            new GetUserByIdQuery
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
    }
}