using FluentResults;
using MediatR;

public class DeleteRentalCommand : IRequest<Result>
{
    public required int Id { get; set; }
}