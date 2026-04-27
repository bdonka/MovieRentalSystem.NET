using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IDbContext 
    {
        public DbSet<Genre> Genres { get; }
        public DbSet<Movie> Movies { get; }
        public DbSet<MoviePhysicalCopy> MoviePhysicalCopies { get; }
        public DbSet<Rental> Rentals { get; }
        public DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); 
    }
}
