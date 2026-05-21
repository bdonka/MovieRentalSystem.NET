using Microsoft.AspNetCore.Identity;

namespace MovieRentalSystem.NET.Domain.Entities;
public class User : IdentityUser
{
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public List<Rental> Rentals { get; set; } = new();
}
