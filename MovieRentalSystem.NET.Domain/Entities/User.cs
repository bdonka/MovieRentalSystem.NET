namespace MovieRentalSystem.NET.Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; } = null;
    public string? Password { get; set; } = null;
    public string? Role { get; set; } = "User";
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public List<Rental> Rentals { get; } = [];
}
