using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Tests.Common;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Configurations;

public class GenreConfigurationTests
{
    [Fact]
    public async Task ShouldNotAllowNullGenreName()
    {
        var context = DbContextFactory.CreateSqlite();

        var genre = new Genre
        {
            Name = null!
        };

        context.Genres.Add(genre);

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            context.SaveChangesAsync());
    }

    [Fact]
    public async Task ShouldNotAllowNameLongThan100Chars()
    {
        var context = DbContextFactory.CreateSqlite();

        var genre = new Genre
        {
            Name = new string('A', 101)
        };

        context.Genres.Add(genre);

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            context.SaveChangesAsync());
    }

    [Fact]
    public async Task ShouldNotAllowDuplicateGenreNames()
    {
        var context = DbContextFactory.CreateSqlite();

        context.Genres.Add(new Genre { Name = "Action" });
        context.Genres.Add(new Genre { Name = "Action" });

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            context.SaveChangesAsync());
    }

    [Fact]
    public async Task ShouldCreateManyToManyRelationBetweenGenreAndMovie()
    {
        var context = DbContextFactory.CreateSqlite();

        var movie = new Movie { Title = "Matrix" };
        var genre = new Genre { Name = "Sci-Fi" };

        genre.Movies.Add(movie);

        context.Genres.Add(genre);
        await context.SaveChangesAsync();

        var saved = await context.Genres
            .Include(g => g.Movies)
            .FirstAsync();

        saved.Movies.Should().HaveCount(1);
        saved.Movies.First().Title.Should().Be("Matrix");
    }
}
