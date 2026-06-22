using FluentAssertions;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Tests.Common;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Tests.Mappings;

public class UserMappingTests
{
    [Fact]
    public void MapToUserDto_ValidUser_ReturnsCorrectDto()
    {
        // Arrange
        var user = TestData.CreateUser();

        // Act
        var dto = user.MapToUserDto();

        // Assert
        dto.Should().BeEquivalentTo(new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.DateRegistered
        });
    }


    [Fact]
    public void MapToUserDto_UserWithRentals_ReturnsRentals()
    {
        // Arrange
        var user = TestData.CreateUser();

        var movie = TestData.CreateMovie();

        var copy = TestData.CreateMovieCopy(
            1,
            movie);

        user.Rentals = new List<Rental>
        {
            TestData.CreateRental(
                1,
                user,
                copy)
        };


        // Act
        var dto = user.MapToUserDto();


        // Assert
        dto.Rentals.Should()
            .HaveCount(1);

        dto.Rentals.First().Should()
            .BeEquivalentTo(new
            {
                Id = user.Rentals.First().Id,
                UserId = user.Id,
                MoviePhysicalCopyId = copy.Id
            });
    }


    [Fact]
    public void MapToUserDto_UserWithoutRentals_ReturnsEmptyRentalList()
    {
        // Arrange
        var user = TestData.CreateUser();

        user.Rentals = null;


        // Act
        var dto = user.MapToUserDto();


        // Assert
        dto.Rentals.Should()
            .NotBeNull();

        dto.Rentals.Should()
            .BeEmpty();
    }
}