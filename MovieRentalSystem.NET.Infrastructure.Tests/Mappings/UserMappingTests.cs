using Bogus;
using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Mapping;

public class UserMappingTests
{
    private readonly Faker<User> _userFaker;
    private readonly Faker<UserDto> _userDtoFaker;

    public UserMappingTests()
    {
        _userFaker = new Faker<User>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.DateRegistered, f => f.Date.Past())
            .RuleFor(x => x.Rentals, _ => new List<Rental>());


        _userDtoFaker = new Faker<UserDto>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.DateRegistered, f => f.Date.Past())
            .RuleFor(x => x.Rentals, _ => new List<RentalDto>());
    }


    [Fact]
    public void MapToUserDto_UserEntity_ReturnsCorrectUserDto()
    {
        // Arrange
        var user = _userFaker.Generate();


        // Act
        var dto = user.MapToUserDto();


        // Assert
        dto.Should().BeEquivalentTo(new UserDto
        {
            Email = user.Email,
            DateRegistered = user.DateRegistered,
            Rentals = new List<RentalDto>()
        });
    }


    [Fact]
    public void MapToUserEntity_UserDto_ReturnsCorrectUserEntity()
    {
        // Arrange
        var dto = _userDtoFaker.Generate();


        // Act
        var entity = dto.MapToUserEntity();


        // Assert
        entity.Email.Should()
        .Be(dto.Email);

        entity.DateRegistered.Should()
            .Be(dto.DateRegistered);

        entity.Rentals.Should()
            .BeEmpty();
    }


    [Fact]
    public void MapToUserDto_UserWithRentals_ReturnsRentals()
    {
        // Arrange
        var user = _userFaker.Generate();

        var movie = new Movie
        {
            Id = 1,
            Title = "Test Movie"
        };

        var movieCopy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = movie.Id,
            Movie = movie
        };

        user.Rentals = new List<Rental>
    {
        new Rental
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            MoviePhysicalCopyId = movieCopy.Id,
            MoviePhysicalCopy = movieCopy
        }
    };


        // Act
        var dto = user.MapToUserDto();


        // Assert
        dto.Rentals.Should()
            .HaveCount(1);
    }
}