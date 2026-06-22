using FluentAssertions;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Tests.Mappings;

public class MovieMappingTests
{
    [Fact]
    public void MapToMovieDto_WithFullData_MapsCorrectly()
    {
        // Arrange
        var movie = new Movie
        {
            Id = 1,
            Title = "Matrix",
            Description = "Sci-fi",
            ReleaseYear = 1999,
            RentalPrice = 10m,
            Genres = new List<Genre>
            {
                new() { Id = 1, Name = "Action" },
                new() { Id = 2, Name = "Sci-Fi" }
            },
            PhysicalCopies = new List<MoviePhysicalCopy>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            }
        };

        // Act
        var result = movie.MapToMovieDto();

        // Assert
        result.Id.Should().Be(movie.Id);
        result.Title.Should().Be(movie.Title);
        result.Description.Should().Be(movie.Description);
        result.ReleaseYear.Should().Be(movie.ReleaseYear);
        result.RentalPrice.Should().Be(movie.RentalPrice);

        result.Genres.Should().HaveCount(2);
        result.PhysicalCopies.Should().HaveCount(2);
    }

    [Fact]
    public void MapToMovieDto_WithEmptyCollections_ReturnsEmptyLists()
    {
        // Arrange
        var movie = new Movie
        {
            Id = 1,
            Title = "Matrix",
            Description = "Sci-fi",
            ReleaseYear = 1999,
            RentalPrice = 10m,
            Genres = new List<Genre>(),
            PhysicalCopies = new List<MoviePhysicalCopy>()
        };

        // Act
        var result = movie.MapToMovieDto();

        // Assert
        result.Genres.Should().BeEmpty();
        result.PhysicalCopies.Should().BeEmpty();
    }

    [Fact]
    public void MapToMovieDto_WithNullCollections_ReturnsEmptyLists()
    {
        // Arrange
        var movie = new Movie
        {
            Id = 1,
            Title = "Matrix",
            Description = "Sci-fi",
            ReleaseYear = 1999,
            RentalPrice = 10m,
            Genres = null!,
            PhysicalCopies = null!
        };

        // Act
        var result = movie.MapToMovieDto();

        // Assert
        result.Genres.Should().BeEmpty();
        result.PhysicalCopies.Should().BeEmpty();
    }
}