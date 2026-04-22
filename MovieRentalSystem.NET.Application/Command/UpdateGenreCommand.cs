using FluentResults;
using MediatR;

public class UpdateGenreCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}