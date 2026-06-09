using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class DeleteGenreCommandHandlerTests
{
    private static IDbContext CreateDb(List<Genre> genres)
    {
        var db = Substitute.For<IDbContext>();

        var genreDbSet = genres.BuildMockDbSet();
        db.Genres.Returns(genreDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);
        return db;
    }

    private static DeleteGenreCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<DeleteGenreCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingGenreId_DeletesGenreSuccessfully()
    {
        // Arrange
        var genres = new List<Genre>
        {
            new() { Id = 1, Name = "x" }
        };

        var db = CreateDb(genres);
        var handler = CreateHandler(db);

        // Act
        var result = await handler.Handle(
            new DeleteGenreCommand { Id = 1 },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        db.Genres.Received(1).Remove(Arg.Is<Genre>(g => g.Id == 1));

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistingGenreId_ReturnsGenreNotFoundError()
    {
        // Arrange
        var genres = new List<Genre>();

        var handler = CreateHandler(CreateDb(genres));

        // Act
        var result = await handler.Handle(
            new DeleteGenreCommand { Id = 999 },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<GenreNotFoundError>();
    }
}