using Bogus;
using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Mapping;

public class MovieMappingTests
{
    private readonly Faker<Movie> _movieFaker;
    private readonly Faker<Genre> _genreFaker;
    private readonly Faker<MoviePhysicalCopy> _copyFaker;

    public MovieMappingTests()
    {
        _genreFaker = new Faker<Genre>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);

        _copyFaker = new Faker<MoviePhysicalCopy>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Code, f => f.Random.AlphaNumeric(8));

        _movieFaker = new Faker<Movie>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.ReleaseYear, f => f.Date.Past(20).Year)
            .RuleFor(x => x.RentalPrice, f => f.Random.Decimal(1, 20))
            .RuleFor(x => x.Genres, _ => new List<Genre>())
            .RuleFor(x => x.PhysicalCopies, _ => new List<MoviePhysicalCopy>());
    }

    [Fact]
    public void MapToMovieDto_MovieEntity_ReturnsCorrectMovieDto()
    {
        // Arrange
        var movie = _movieFaker.Generate();
        movie.Genres = _genreFaker.Generate(2);
        movie.PhysicalCopies = _copyFaker.Generate(2);

        // Act
        var dto = movie.MapToMovieDto();

        // Assert
        dto.Should().BeEquivalentTo(new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            RentalPrice = movie.RentalPrice,
            Genres = movie.Genres.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList(),
            PhysicalCopies = movie.PhysicalCopies.Select(c => new MoviePhysicalCopyDto
            {
                Id = c.Id,
                Code = c.Code
            }).ToList()
        });
    }

    [Fact]
    public void MapToMovieEntity_MovieDto_ReturnsCorrectEntity()
    {
        // Arrange
        var dto = new MovieDto
        {
            Id = 10,
            Title = "Inception",
            Description = "Dreams",
            ReleaseYear = 2010,
            RentalPrice = 12.5m,
            Genres = new List<GenreDto>(),
            PhysicalCopies = new List<MoviePhysicalCopyDto>()
        };

        // Act
        var entity = dto.MapToMovieEntity();

        // Assert
        entity.Should().BeEquivalentTo(new Movie
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            ReleaseYear = dto.ReleaseYear,
            RentalPrice = dto.RentalPrice
        });
    }

    [Fact]
    public void MapToMovieDto_ShouldHandleEmptyCollections()
    {
        // Arrange
        var movie = _movieFaker.Generate();
        movie.Genres = new List<Genre>();
        movie.PhysicalCopies = new List<MoviePhysicalCopy>();

        // Act
        var dto = movie.MapToMovieDto();

        // Assert
        dto.Genres.Should().BeEmpty();
        dto.PhysicalCopies.Should().BeEmpty();
    }
}