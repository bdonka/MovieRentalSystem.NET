using FluentResults;
using MediatR;
public class UpdateUserCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required string Email { get; set; } = null!;
}