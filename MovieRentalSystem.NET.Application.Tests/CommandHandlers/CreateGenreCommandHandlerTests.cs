using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class CreateGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();
        db.Genres.Returns(genres.BuildMock());
        return db;
    }

    private static CreateGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<CreateGenreCommandHandler>>());

    [Fact]
    public async Task Should_Create_Genre()
    {
        var genres = new List<Genre>();
        var db = CreateDb(genres);
        var handler = CreateHandler(db);

        var command = new CreateGenreCommand
        {
            Name = new Faker().Commerce.Department()
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        genres.Should().ContainSingle();
    }
}