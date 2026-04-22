using FluentResults;
using MediatR;

public class DeleteMovieCommand : IRequest<Result>
{
    public required int Id  { get; set; }
}