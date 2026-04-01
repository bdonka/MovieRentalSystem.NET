using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(m => m.Description )
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(m => m.RentalPrice)
            .HasPrecision(10, 2);
        builder.HasIndex(m => new { m.Title, m.ReleaseYear })
            .IsUnique();
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Movie_RentalPrice",
                "RentalPrice >= 0"
            );
        });
        builder.HasMany(m => m.Genres)
            .WithMany(m => m.Movies);

        builder.HasMany(m => m.PhysicalCopies)
            .WithOne(pc => pc.Movie)
            .HasForeignKey(pc => pc.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
