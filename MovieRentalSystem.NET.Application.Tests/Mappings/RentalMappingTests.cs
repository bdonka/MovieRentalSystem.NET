using FluentAssertions;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Tests.Mapping;

public class RentalMappingTests
{
    [Fact]
    public void MapToRentalDto_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var user = TestData.CreateUser();
        var movie = TestData.CreateMovie();
        var copy = TestData.CreateMovieCopy(1, movie);

        var rental = TestData.CreateRental(
            id: 1,
            user: user,
            copy: copy,
            status: RentalStatus.Active);

        rental.OrderDate = DateTime.UtcNow.AddDays(-10);
        rental.RentalStartDate = DateTime.UtcNow.AddDays(-9);
        rental.DueDate = DateTime.UtcNow.AddDays(1);
        rental.ReturnDate = null;
        rental.TotalPrice = 99.99m;

        // Act
        var dto = rental.MapToRentalDto();

        // Assert
        dto.Should().NotBeNull();

        dto.Id.Should().Be(rental.Id);
        dto.UserId.Should().Be(rental.UserId);
        dto.MoviePhysicalCopyId.Should().Be(rental.MoviePhysicalCopyId);

        dto.OrderDate.Should().Be(rental.OrderDate);
        dto.RentalStartDate.Should().Be(rental.RentalStartDate);
        dto.DueDate.Should().Be(rental.DueDate);
        dto.ReturnDate.Should().Be(rental.ReturnDate);
        dto.TotalPrice.Should().Be(rental.TotalPrice);
        dto.Status.Should().Be(rental.Status);

        dto.MoviePhysicalCopy.Should().NotBeNull();
        dto.MoviePhysicalCopy.Id.Should().Be(copy.Id);
        dto.MoviePhysicalCopy.Code.Should().Be(copy.Code);
        dto.MoviePhysicalCopy.Status.Should().Be(copy.Status);
    }

    [Fact]
    public void MapToRentalDto_ShouldThrow_WhenMoviePhysicalCopyIsNull()
    {
        // Arrange
        var user = TestData.CreateUser();

        var rental = TestData.CreateRental(
            id: 1,
            user: user,
            copy: TestData.CreateMovieCopy(1, TestData.CreateMovie()));

        rental.MoviePhysicalCopy = null!;

        // Act
        Action act = () => rental.MapToRentalDto();

        // Assert
        act.Should().Throw<NullReferenceException>();
    }
}