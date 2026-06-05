using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Mapping;
using Bogus;
using Xunit;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Mapping;

public class GenreMappingTests
{
    private readonly Faker<Genre> _genreFaker;
    private readonly Faker<GenreDto> _genreDtoFaker;

    public GenreMappingTests()
    {
        _genreFaker = new Faker<Genre>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);

        _genreDtoFaker = new Faker<GenreDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);
    }

    [Fact]
    public void Should_Map_Genre_To_GenreDto_Correctly()
    {
        // Arrange
        var genre = _genreFaker.Generate();

        // Act
        var dto = genre.MapToGenreDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(genre.Id);
        dto.Name.Should().Be(genre.Name);

        dto.Should().BeEquivalentTo(new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        });
    }

    [Fact]
    public void Should_Map_GenreDto_To_Genre_Entity_Correctly()
    {
        // Arrange
        var dto = _genreDtoFaker.Generate();

        // Act
        var entity = dto.MapToGenreEntity();

        // Assert
        entity.Should().NotBeNull();
        entity.Id.Should().Be(dto.Id);
        entity.Name.Should().Be(dto.Name);

        entity.Should().BeEquivalentTo(new Genre
        {
            Id = dto.Id,
            Name = dto.Name
        });
    }

    [Fact]
    public void Mapping_Should_Be_Symmetric()
    {
        // Arrange
        var genre = _genreFaker.Generate();

        // Act
        var dto = genre.MapToGenreDto();
        var mappedBack = dto.MapToGenreEntity();

        // Assert
        mappedBack.Should().NotBeNull();
        mappedBack.Id.Should().Be(genre.Id);
        mappedBack.Name.Should().Be(genre.Name);
    }
}