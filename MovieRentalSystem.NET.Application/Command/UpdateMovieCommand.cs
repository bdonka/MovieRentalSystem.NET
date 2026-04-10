using FluentResults;
using MediatR;

public class UpdateMovieCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public decimal RentalPrice { get; set; }
}