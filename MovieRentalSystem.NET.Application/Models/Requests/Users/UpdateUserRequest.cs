namespace MovieRentalSystem.NET.Application.Models.Requests.Users
{
    public class UpdateUserRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
