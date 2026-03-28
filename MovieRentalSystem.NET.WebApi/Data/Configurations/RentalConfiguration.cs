using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Data.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(m => m.Status)
            .IsRequired();
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Rental_TotalPrice", "TotalPrice >= 0");
            t.HasCheckConstraint(
                "CK_Rental_RentalStartDate",
                "RentalStartDate IS NULL OR DueDate IS NULL OR RentalStartDate <= DueDate"
            );
        });
        builder.HasOne(r => r.User)
            .WithMany(u => u.Rentals)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.MoviePhysicalCopy)
            .WithMany()
            .HasForeignKey(r => r.MoviePhysicalCopyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
