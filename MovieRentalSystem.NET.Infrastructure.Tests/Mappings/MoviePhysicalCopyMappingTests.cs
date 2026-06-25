using Bogus;
using FluentAssertions;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using MovieRentalSystem.NET.Infrastructure.Mapping;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Mapping;

public class MoviePhysicalCopyMappingTests
{
    private readonly Faker<MoviePhysicalCopy> _copyFaker;
    private readonly Faker<MoviePhysicalCopyDto> _dtoFaker;
    private readonly Faker<Movie> _movieFaker;

    public MoviePhysicalCopyMappingTests()
    {
        _movieFaker = new Faker<Movie>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.ReleaseYear, f => f.Date.Past(20).Year)
            .RuleFor(x => x.RentalPrice, f => f.Random.Decimal(1, 20))
            .RuleFor(x => x.Genres, _ => new List<Genre>())
            .RuleFor(x => x.PhysicalCopies, _ => new List<MoviePhysicalCopy>());

        _copyFaker = new Faker<MoviePhysicalCopy>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.MovieId, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Code, f => $"COPY-{f.Random.AlphaNumeric(6)}")
            .RuleFor(x => x.Status, f => f.PickRandom<MovieCopyStatus>())
            .RuleFor(x => x.Movie, f => _movieFaker.Generate());

        _dtoFaker = new Faker<MoviePhysicalCopyDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.MovieId, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Code, f => $"COPY-{f.Random.AlphaNumeric(6)}")
            .RuleFor(x => x.Status, f => f.PickRandom<MovieCopyStatus>());
    }

    [Fact]
    public void MapToMoviePhysicalCopyDto_Entity_ReturnsCorrectDto()
    {
        var copy = new MoviePhysicalCopy
        {
            Id = 1,
            MovieId = 10,
            Code = "COPY-001",
            Status = MovieCopyStatus.Rented
        };

        var dto = copy.MapToMoviePhysicalCopyDto();

        dto.Should().BeEquivalentTo(new MoviePhysicalCopyDto
        {
            Id = 1,
            MovieId = 10,
            Code = "COPY-001",
            Status = MovieCopyStatus.Rented
        });
    }

    [Fact]
    public void MapToMoviePhysicalCopyDto_ShouldWorkWithoutMovieNavigation()
    {
        // Arrange
        var copy = _copyFaker.Generate();
        copy.Movie = null!;

        // Act
        var dto = copy.MapToMoviePhysicalCopyDto();

        // Assert
        dto.MovieId.Should().Be(copy.MovieId);
        dto.Code.Should().Be(copy.Code);
    }
}