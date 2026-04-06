namespace MovieRentalSystem.NET.Application.Models.Requests.Users;

public class CreateUserRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
