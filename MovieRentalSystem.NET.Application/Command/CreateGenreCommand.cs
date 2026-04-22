using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public class CreateGenreCommand : IRequest<Result<GenreDto>>
{
    public required string Name { get; set; }
}