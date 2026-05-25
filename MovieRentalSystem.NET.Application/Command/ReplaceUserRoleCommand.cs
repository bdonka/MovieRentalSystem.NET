using FluentResults;
using MediatR;
public class ReplaceUserRoleCommand : IRequest<Result>
{
    public required string Id { get; set; } = null!;
    public required string Role { get; set; }
}