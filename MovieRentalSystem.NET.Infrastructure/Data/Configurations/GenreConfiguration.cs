using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Infrastructure.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(g => g.Name)
            .IsUnique();
        builder.HasMany(g => g.Movies)
                .WithMany(m => m.Genres);
    }
}
