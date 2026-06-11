using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class CreateGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();

        var genreDbSet = genres.BuildMockDbSet();
        db.Genres.Returns(genreDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static CreateGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<CreateGenreCommandHandler>>());

    [Fact]
    public async Task Handle_ValidGenreName_CreatesGenreSuccessfully()
    {
        // Arrange
        var genres = new List<Genre>();
        var db = CreateDb(genres);
        var handler = CreateHandler(db);

        var command = new CreateGenreCommand
        {
            Name = new Faker().Commerce.Department()
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        db.Genres.Received(1).Add(Arg.Is<Genre>(g => g.Name == command.Name));

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ExistingGenreName_ReturnsGenreAlreadyExistsError()
    {
        // Arrange
        var existingGenre = new Genre { Id = 1, Name = "Action" };

        var genres = new List<Genre> { existingGenre };

        var db = CreateDb(genres);
        var handler = CreateHandler(db);

        var command = new CreateGenreCommand
        {
            Name = "Action"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<GenreAlreadyExistsError>();
        genres.Should().ContainSingle(g => g.Name == existingGenre.Name);
    }
}