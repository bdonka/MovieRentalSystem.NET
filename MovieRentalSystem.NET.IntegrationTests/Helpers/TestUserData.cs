namespace MovieRentalSystem.NET.IntegrationTests.Helpers;

public record TestUserData(
    string UserName,
    string Email,
    string Password)
{
    public static TestUserData Customer()
    {
        var id = Guid.NewGuid();

        return new(
            $"customer_{id}",
            $"customer_{id}@test.com",
            "Password123!");
    }

    public static TestUserData Worker()
    {
        var id = Guid.NewGuid();

        return new(
            $"worker_{id}",
            $"worker_{id}@test.com",
            "Worker123!");
    }

    public static TestUserData Admin()
    {
        var id = Guid.NewGuid();

        return new(
            $"admin_{id}",
            $"admin_{id}@test.com",
            "Admin123!");
    }
}
