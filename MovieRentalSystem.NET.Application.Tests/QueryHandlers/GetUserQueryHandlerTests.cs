using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.QueryHandlers;

public class GetUserQueryHandlerTests
{
    private static IDbContext CreateDbContext(
        List<User> users)
    {
        var db = Substitute.For<IDbContext>();
        var userDbSet = users.BuildMockDbSet();

        db.Users.Returns(userDbSet);

        return db;
    }


    private static GetUserQueryHandler CreateHandler(
        IDbContext dbContext)
        => new(
            dbContext,
            Substitute.For<ILogger<GetUserQueryHandler>>());



    [Fact]
    public async Task Handle_ExistingUsers_ReturnsPagedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            TestData.CreateUser(),
            TestData.CreateUser(),
            TestData.CreateUser()
        };

        var db = CreateDbContext(users);

        var handler = CreateHandler(db);


        // Act
        var result = await handler.Handle(
            new GetUserQuery
            {
                PageNumber = 1,
                PageSize = 10
            },
            CancellationToken.None);


        // Assert
        result.Should().NotBeNull();

        result.TotalRecords.Should()
            .Be(3);

        result.Data.Should()
            .HaveCount(3);
    }



    [Fact]
    public async Task Handle_PageSizeOne_ReturnsCorrectPage()
    {
        // Arrange
        var users = new List<User>
        {
            TestData.CreateUser(),
            TestData.CreateUser(),
            TestData.CreateUser()
        };

        var db = CreateDbContext(users);

        var handler = CreateHandler(db);


        // Act
        var result = await handler.Handle(
            new GetUserQuery
            {
                PageNumber = 2,
                PageSize = 1
            },
            CancellationToken.None);


        // Assert
        result.TotalRecords.Should()
            .Be(3);

        result.Data.Should()
            .ContainSingle();
    }



    [Fact]
    public async Task Handle_NoUsers_ReturnsEmptyPagedResponse()
    {
        // Arrange
        var db = CreateDbContext(
            new List<User>());

        var handler = CreateHandler(db);


        // Act
        var result = await handler.Handle(
            new GetUserQuery
            {
                PageNumber = 1,
                PageSize = 10
            },
            CancellationToken.None);


        // Assert
        result.Should().NotBeNull();

        result.TotalRecords.Should()
            .Be(0);

        result.Data.Should()
            .BeEmpty();
    }
}