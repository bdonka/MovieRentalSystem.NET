using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();
    public DbSet<MoviePhysicalCopy> MoviePhysicalCopies => Set<MoviePhysicalCopy>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<User> Users => Set<User>();

}


