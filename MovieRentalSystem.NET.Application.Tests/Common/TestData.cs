using Bogus;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Tests.Common;

public static class TestData
{
    public static Faker<Genre> GenreFaker()
        => new Faker<Genre>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Name, f => f.Commerce.Department());

    public static List<Genre> Genres(int count)
        => GenreFaker().Generate(count);

    public static Faker<Movie> MovieFaker()
        => new Faker<Movie>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Genres, _ => new List<Genre>());
}