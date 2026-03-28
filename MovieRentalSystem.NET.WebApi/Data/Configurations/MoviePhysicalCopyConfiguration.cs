using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Enums;

namespace MovieRentalSystem.NET.WebApi.Data.Configurations;

public class MoviePhysicalCopyConfiguration : IEntityTypeConfiguration<MoviePhysicalCopy>
{
    public void Configure(EntityTypeBuilder<MoviePhysicalCopy> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Code)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(m => m.Code).IsUnique();
        builder.Property(m => m.Status)
            .IsRequired();
        builder.HasOne(m => m.Movie)
            .WithMany(m => m.PhysicalCopies)
            .HasForeignKey(m => m.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
