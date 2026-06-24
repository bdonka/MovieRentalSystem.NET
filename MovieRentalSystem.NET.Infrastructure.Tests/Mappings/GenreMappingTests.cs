using Bogus;
using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Mapping;

public class RentalMappingTests
{
    private readonly Faker<Rental> _rentalFaker;
    private readonly Faker<RentalDto> _rentalDtoFaker;
    private readonly Faker<MoviePhysicalCopy> _copyFaker;

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
            .RuleFor(x => x.ReturnDate, f => null)
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal(1, 500))
            .RuleFor(x => x.Status, f => f.PickRandom<RentalStatus>());

        _rentalDtoFaker = new Faker<RentalDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.UserId, f => Guid.NewGuid().ToString())
            .RuleFor(x => x.MoviePhysicalCopyId, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.MoviePhysicalCopy, new MoviePhysicalCopyDto
            {
                Id = 1,
                Code = "COPY-001",
                Status = MovieCopyStatus.Available
            })
            .RuleFor(x => x.OrderDate, f => f.Date.Past())
            .RuleFor(x => x.RentalStartDate, f => f.Date.Past())
            .RuleFor(x => x.DueDate, f => f.Date.Future())
            .RuleFor(x => x.ReturnDate, f => null)
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal(1, 500))
            .RuleFor(x => x.Status, f => f.PickRandom<RentalStatus>());
    }

    [Fact]
    public void MapToRentalDto_RentalEntity_ReturnsCorrectDto()
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
    public void MapToRentalEntity_RentalDto_ReturnsCorrectEntity()
    {
        // Arrange
        var rental = _rentalFaker.Generate();

        var dto = new RentalDto
        {
            Id = rental.Id,
            UserId = rental.UserId,
            MoviePhysicalCopyId = rental.MoviePhysicalCopyId,
            OrderDate = rental.OrderDate,
            RentalStartDate = rental.RentalStartDate,
            DueDate = rental.DueDate,
            ReturnDate = rental.ReturnDate,
            TotalPrice = rental.TotalPrice,
            Status = rental.Status
        };

        // Act
        var entity = dto.MapToRentalEntity();

        // Assert
        entity.Should().BeEquivalentTo(new Rental
        {
            Id = dto.Id,
            UserId = dto.UserId,
            MoviePhysicalCopyId = dto.MoviePhysicalCopyId,
            OrderDate = dto.OrderDate,
            RentalStartDate = dto.RentalStartDate,
            DueDate = dto.DueDate,
            ReturnDate = dto.ReturnDate,
            TotalPrice = dto.TotalPrice,
            Status = dto.Status
        });
    }
}