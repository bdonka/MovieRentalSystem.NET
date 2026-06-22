using FluentResults;
using MediatR;

public record DeleteMovieCommand(int Id) : IRequest<Result>{}