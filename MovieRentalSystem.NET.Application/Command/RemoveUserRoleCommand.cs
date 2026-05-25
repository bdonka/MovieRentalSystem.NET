using FluentResults;
using MediatR;

public class RemoveUserRoleCommand : IRequest<Result>
{
    public required string Id { get; set; }
    public required string Role { get; set; }
}