namespace MovieRentalSystem.NET.WebApi.Models.Requests.Users
{
    public class ReplaceUserRoleRequest
    {
        public string Id { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
