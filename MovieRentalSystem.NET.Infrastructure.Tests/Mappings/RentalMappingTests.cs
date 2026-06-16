using Bogus;
using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Tests.Mapping;

public class RentalMappingTests
{
    private readonly Faker<MoviePhysicalCopy> _copyFaker;
    private readonly Faker<Rental> _rentalFaker;

    public RentalMappingTests()
    {
        _copyFaker = new Faker<MoviePhysicalCopy>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Code, f => $"COPY-{f.Random.AlphaNumeric(6)}")
            .RuleFor(x => x.Status, f => f.PickRandom<MovieCopyStatus>())
            .RuleFor(x => x.MovieId, f => f.Random.Int(1, 1000));

        _rentalFaker = new Faker<Rental>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.UserId, f => Guid.NewGuid().ToString())
            .RuleFor(x => x.MoviePhysicalCopyId, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.MoviePhysicalCopy, _ => _copyFaker.Generate())
            .RuleFor(x => x.OrderDate, f => f.Date.Past())
            .RuleFor(x => x.RentalStartDate, f => f.Date.Past())
            .RuleFor(x => x.DueDate, f => f.Date.Future())
            .RuleFor(x => x.ReturnDate, _ => null)
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal(1, 500))
            .RuleFor(x => x.Status, f => f.PickRandom<RentalStatus>());
    }

    [Fact]
    public void MapToRentalDto_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var rental = _rentalFaker.Generate();

        // Act
        var dto = rental.MapToRentalDto();

        // Assert
        dto.Should().BeEquivalentTo(new RentalDto
        {
            Id = rental.Id,
            UserId = rental.UserId,
            MoviePhysicalCopyId = rental.MoviePhysicalCopyId,
            MoviePhysicalCopy = new MoviePhysicalCopyDto
            {
                Id = rental.MoviePhysicalCopy.Id,
                Code = rental.MoviePhysicalCopy.Code,
                Status = rental.MoviePhysicalCopy.Status
            },
            OrderDate = rental.OrderDate,
            RentalStartDate = rental.RentalStartDate,
            DueDate = rental.DueDate,
            ReturnDate = rental.ReturnDate,
            TotalPrice = rental.TotalPrice,
            Status = rental.Status
        });
    }

    [Fact]
    public void MapToRentalDto_ShouldThrow_WhenMoviePhysicalCopyIsNull()
    {
        // Arrange
        var rental = _rentalFaker.Generate();
        rental.MoviePhysicalCopy = null!;

        // Act
        Action act = () => rental.MapToRentalDto();

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void MapToRentalDto_ShouldMapMoviePhysicalCopyCorrectly()
    {
        // Arrange
        var rental = _rentalFaker.Generate();

        // Act
        var dto = rental.MapToRentalDto();

        // Assert
        dto.MoviePhysicalCopy.Should().BeEquivalentTo(new MoviePhysicalCopyDto
        {
            Id = rental.MoviePhysicalCopy.Id,
            Code = rental.MoviePhysicalCopy.Code,
            Status = rental.MoviePhysicalCopy.Status
        });
    }
}