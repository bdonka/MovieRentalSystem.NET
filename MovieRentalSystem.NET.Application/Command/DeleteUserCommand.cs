using FluentResults;
using MediatR;

public class DeleteUserCommand : IRequest<Result>
{
    public required string Id { get; set; } = null!;
}