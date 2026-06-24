using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using NSubstitute;

namespace MovieRentalSystem.NET.Application.Tests.CommandHandlers;

public class DeleteMoviePhysicalCopyCommandHandlerTests
{
    private static IDbContext CreateDb(List<MoviePhysicalCopy> copies)
    {
        var db = Substitute.For<IDbContext>();

        var copyDbSet = copies.BuildMockDbSet();

        db.MoviePhysicalCopies.Returns(copyDbSet);
        db.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        return db;
    }

    private static DeleteMoviePhysicalCopyCommandHandler CreateHandler(IDbContext db)
        => new(db, Substitute.For<ILogger<DeleteMoviePhysicalCopyCommandHandler>>());

    [Fact]
    public async Task Handle_ExistingCopy_DeletesSuccessfully()
    {
        // Arrange
        var movie = TestData.CreateMovie(1);

        var copy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = movie.Id,
            Movie = movie,
            Code = "COPY-001",
            Status = MovieCopyStatus.Available
        };

        var copies = new List<MoviePhysicalCopy> { copy };

        var db = CreateDb(copies);
        var handler = CreateHandler(db);

        var command = new DeleteMoviePhysicalCopyCommand
        {
            Id = 1
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        db.MoviePhysicalCopies.Received(1).Remove(
            Arg.Is<MoviePhysicalCopy>(c => c.Id == 1));

        await db.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistingCopy_ReturnsNotFoundError()
    {
        // Arrange
        var copies = new List<MoviePhysicalCopy>();

        var db = CreateDb(copies);
        var handler = CreateHandler(db);

        var command = new DeleteMoviePhysicalCopyCommand
        {
            Id = 999
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should().BeOfType<MoviePhysicalCopyNotFoundError>();

        copies.Should().BeEmpty();

        await db.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}