using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MovieRentalSystem.NET.Application.Common.Errors;
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
    public async Task Handle_ExistingGenre_UpdatesNameSuccessfully()
    {
        // Arrange
        var genre = new Faker<Genre>()
            .RuleFor(x => x.Id, 1)
            .RuleFor(x => x.Name, f => f.Commerce.Department())
            .Generate();

        var db = CreateDb(new List<Genre> { genre });
        var handler = CreateHandler(db);

        var newName = new Faker().Commerce.Department();

        // Act
        var result = await handler.Handle(new UpdateGenreCommand
        {
            Id = 1,
            Name = newName
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        genre.Name.Should().Be(newName);
    }

    [Fact]
    public async Task Handle_NonExistingGenre_ReturnsGenreNotFoundError()
    {
        // Arrange
        var db = CreateDb(new List<Genre>());
        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new UpdateGenreCommand
        {
            Id = 1,
            Name = "New Name"
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e is GenreNotFoundError);
    }

    [Fact]
    public async Task Handle_DuplicateGenreName_ReturnsGenreAlreadyExistsError()
    {
        // Arrange
        var genre1 = new Genre { Id = 1, Name = "Action" };
        var genre2 = new Genre { Id = 2, Name = "Comedy" };

        var db = CreateDb(new List<Genre> { genre1, genre2 });
        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(new UpdateGenreCommand
        {
            Id = 2,
            Name = "Action"
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e is GenreAlreadyExistsError);
    }
}