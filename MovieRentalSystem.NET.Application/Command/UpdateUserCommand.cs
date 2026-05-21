using FluentResults;
using MediatR;
public class UpdateUserCommand : IRequest<Result>
{
    public required string Id { get; set; } = null!;
    public required string UserName { get; set; } = null!;
    public required string Email { get; set; } = null!;
}