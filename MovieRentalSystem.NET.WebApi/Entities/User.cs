namespace MovieRentalSystem.NET.WebApi.Entities;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; } = null;
    public string? Password { get; set; } = null;
    public string? Role { get; set; } = "User";
    public DateTime DateRegistered { get; set; } = DateTime.Now;
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
