using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class UpdateGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();
        db.Genres.Returns(genres.BuildMock());
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static UpdateGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<UpdateGenreCommandHandler>>());

    [Fact]
    public async Task Should_Update_Genre()
    {
        var genre = new Faker<Genre>()
            .RuleFor(x => x.Id, 1)
            .RuleFor(x => x.Name, f => f.Commerce.Department())
            .Generate();

        var db = CreateDb(new List<Genre> { genre });
        var handler = CreateHandler(db);

        var newName = new Faker().Commerce.Department();

        var result = await handler.Handle(new UpdateGenreCommand
        {
            Id = 1,
            Name = newName
        }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        genre.Name.Should().Be(newName);
    }
}