using FluentResults;
using MediatR;

public class DeleteGenreCommand : IRequest<Result>
{
    public required int Id { get; set; }
}