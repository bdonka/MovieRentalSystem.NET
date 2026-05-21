namespace MovieRentalSystem.NET.Application.Dtos;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Email { get; set; } = null;
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public List<RentalDto> Rentals { get; set; } = new();
}

public class CreateUserDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    //public string? Role { get; set; } = "User";
    //public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
}
