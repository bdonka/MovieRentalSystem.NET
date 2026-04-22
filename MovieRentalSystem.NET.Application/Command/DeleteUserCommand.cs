using FluentResults;
using MediatR;

public class DeleteUserCommand : IRequest<Result>
{
    public required int Id { get; set; }
}