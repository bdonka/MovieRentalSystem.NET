using MediatR;

public class CreateUserCommand : IRequest<int>
{
    public required string Name { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
}