using Bogus;
using FluentAssertions;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class GenreMappingTests
{
    private readonly Faker<Genre> _genreFaker;

    public GenreMappingTests()
    {
        _genreFaker = new Faker<Genre>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 10000))
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);
    }

    [Fact]
    public void MapToGenreDtoShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var genre = _genreFaker.Generate();

        // Act
        var dto = genre.MapToGenreDto();

        // Assert
        dto.Should().NotBeNull();

        dto.Should().BeEquivalentTo(new
        {
            genre.Id,
            genre.Name
        });
    }

    [Fact]
    public void MapToGenreDtoShouldNotAlterData()
    {
        // Arrange
        var genre = _genreFaker.Generate();

        // Act
        var dto = genre.MapToGenreDto();

        // Assert
        dto.Id.Should().Be(genre.Id);
        dto.Name.Should().Be(genre.Name);
    }
}