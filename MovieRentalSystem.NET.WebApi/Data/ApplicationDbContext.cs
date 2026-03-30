using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<MoviePhysicalCopy> MoviePhysicalCopies => Set<MoviePhysicalCopy>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

}


