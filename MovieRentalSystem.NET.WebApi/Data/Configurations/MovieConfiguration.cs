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
        builder.HasIndex(m => m.Title).IsUnique();
        builder.Property(m => m.Description )
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(m => m.RentalPrice)
            .HasPrecision(10, 2);
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Movie_Title_NotEmpty", "LEN(Title) > 0");
            t.HasCheckConstraint("CK_Movie_Description_NotEmpty", "LEN(Description) > 0");
            t.HasCheckConstraint(
                "CK_Movie_ReleaseYear",
                $"ReleaseYear >= 1888 AND ReleaseYear <= {DateTime.UtcNow.Year}"
            );
            t.HasCheckConstraint(
                "CK_Movie_RentalPrice",
                "RentalPrice >= 0"
            );
        });
        builder.HasMany(m => m.MovieGenres)
       .WithOne(mg => mg.Movie)
       .HasForeignKey(mg => mg.MovieId)
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.PhysicalCopies)
               .WithOne(pc => pc.Movie)
               .HasForeignKey(pc => pc.MovieId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
