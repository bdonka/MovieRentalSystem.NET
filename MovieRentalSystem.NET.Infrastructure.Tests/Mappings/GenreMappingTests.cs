using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Mapping;
using Bogus;

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
        var genre = _genreFaker.Generate();

        var dto = new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        };

        // Act
        var entity = dto.MapToGenreEntity();

        // Assert
        entity.Should().BeEquivalentTo(new Genre
        {
            Id = dto.Id,
            Name = dto.Name
        });
    }
}