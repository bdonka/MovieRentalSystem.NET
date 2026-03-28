using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Data.Configurations;

public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasKey(mg => new { mg.MovieId, mg.GenreId });
        builder.HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(mg => new { mg.MovieId, mg.GenreId }).IsUnique();
    }
}
