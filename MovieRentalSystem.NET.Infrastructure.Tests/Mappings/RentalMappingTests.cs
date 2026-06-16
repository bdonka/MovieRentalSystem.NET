using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using Xunit;

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
        rental.DueDate = DateTime.UtcNow.AddDays(2);
        rental.ReturnDate = null;
        rental.TotalPrice = 123.45m;

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
        var movie = TestData.CreateMovie();
        var copy = TestData.CreateMovieCopy(1, movie);

        var rental = TestData.CreateRental(1, user, copy);

        rental.MoviePhysicalCopy = null!;

        // Act
        Action act = () => rental.MapToRentalDto();

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void MapToRentalEntity_ShouldMapAllFieldsExceptNavigationProperties()
    {
        // Arrange
        var user = TestData.CreateUser();
        var movie = TestData.CreateMovie();
        var copy = TestData.CreateMovieCopy(10, movie);

        var rental = TestData.CreateRental(
            id: 5,
            user: user,
            copy: copy,
            status: RentalStatus.Completed);

        rental.TotalPrice = 88.88m;

        var dto = rental.MapToRentalDto();

        // Act (DTO -> Entity)
        var entity = dto.MapToRentalEntity();

        // Assert
        entity.Should().NotBeNull();

        entity.Id.Should().Be(dto.Id);
        entity.UserId.Should().Be(dto.UserId);
        entity.MoviePhysicalCopyId.Should().Be(dto.MoviePhysicalCopyId);

        entity.OrderDate.Should().Be(dto.OrderDate);
        entity.RentalStartDate.Should().Be(dto.RentalStartDate);
        entity.DueDate.Should().Be(dto.DueDate);
        entity.ReturnDate.Should().Be(dto.ReturnDate);
        entity.TotalPrice.Should().Be(dto.TotalPrice);
        entity.Status.Should().Be(dto.Status);

        // navigation intentionally not mapped
        entity.User.Should().BeNull();
        entity.MoviePhysicalCopy.Should().BeNull();
    }

    [Fact]
    public void MapToRentalEntity_ShouldPreserveEnumAndDecimalValues()
    {
        // Arrange
        var dto = new RentalDto
        {
            Id = 1,
            UserId = "user-1",
            MoviePhysicalCopyId = 10,
            OrderDate = DateTime.UtcNow,
            RentalStartDate = DateTime.UtcNow.AddDays(-2),
            DueDate = DateTime.UtcNow.AddDays(5),
            ReturnDate = null,
            TotalPrice = 199.99m,
            Status = RentalStatus.Active,
            MoviePhysicalCopy = new MoviePhysicalCopyDto
            {
                Id = 10,
                Code = "COPY-001",
                Status = MovieCopyStatus.Rented
            }
        };

        // Act
        var entity = dto.MapToRentalEntity();

        // Assert
        entity.Status.Should().Be(RentalStatus.Active);
        entity.TotalPrice.Should().Be(199.99m);
        entity.MoviePhysicalCopyId.Should().Be(10);
    }
}