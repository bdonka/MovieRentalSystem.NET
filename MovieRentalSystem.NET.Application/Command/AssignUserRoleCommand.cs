using FluentResults;
using MediatR;

public class AssignUserRoleCommand : IRequest<Result>
{
    public required string Id { get; set; }
    public required string Role { get; set; }
}