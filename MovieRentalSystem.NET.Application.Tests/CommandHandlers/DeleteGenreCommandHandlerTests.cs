using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable;
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
        db.Genres.Returns(genres.BuildMock());
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

        var handler = CreateHandler(CreateDb(genres));

        // Act
        var result = await handler.Handle(
            new DeleteGenreCommand { Id = 1 },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        genres.Should().NotContain(g => g.Id == 1);
        genres.Should().HaveCount(0);
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