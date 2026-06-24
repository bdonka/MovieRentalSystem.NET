using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Tests.Mappings;

public class MoviePhysicalCopyMappingTests
{
    [Fact]
    public void MapToMoviePhysicalCopyDto_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var copy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = 10,
            Code = "COPY-001",
            Status = MovieCopyStatus.Available
        };

        // Act
        var dto = copy.MapToMoviePhysicalCopyDto();

        // Assert
        dto.Should().BeEquivalentTo(new MoviePhysicalCopyDto
        {
            Id = 1,
            MovieId = 10,
            Code = "COPY-001",
            Status = MovieCopyStatus.Available
        });
    }

    [Fact]
    public void MapToMoviePhysicalCopyDto_ShouldPreserveStatusValues()
    {
        // Arrange
        var copy = new MoviePhysicalCopy
        {
            Id = 2,
            MovieId = 20,
            Code = "COPY-XYZ",
            Status = MovieCopyStatus.Rented
        };

        // Act
        var dto = copy.MapToMoviePhysicalCopyDto();

        // Assert
        dto.Status.Should().Be(MovieCopyStatus.Rented);
    }

    [Fact]
    public void MapToMoviePhysicalCopyDto_ShouldNotThrow_WhenValidEntity()
    {
        // Arrange
        var copy = new MoviePhysicalCopy
        {
            Id = 3,
            MovieId = 30,
            Code = "COPY-TEST",
            Status = MovieCopyStatus.Available
        };

        // Act
        var act = () => copy.MapToMoviePhysicalCopyDto();

        // Assert
        act.Should().NotThrow();
    }
}