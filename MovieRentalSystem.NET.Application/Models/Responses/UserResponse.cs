namespace MovieRentalSystem.NET.Application.Models.Responses;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; } = null;
    public string? Role { get; set; } = "User";
    public DateTime DateRegistered { get; set; } = DateTime.Now;
}
