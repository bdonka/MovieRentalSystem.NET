using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name)
            .IsRequired();
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.Role)
           .IsRequired()
           .HasMaxLength(20)
           .HasDefaultValue("User");
        builder.Property(u => u.DateRegistered)
               .HasDefaultValueSql("GETUTCDATE()");
        builder.HasIndex(u => u.Email)
               .IsUnique();
        builder.HasMany(u => u.Rentals)
               .WithOne(r => r.User)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
