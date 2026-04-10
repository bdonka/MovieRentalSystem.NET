using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public class CreateMovieCommand : IRequest<Result<MovieDto>>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public decimal RentalPrice { get; set; }
}