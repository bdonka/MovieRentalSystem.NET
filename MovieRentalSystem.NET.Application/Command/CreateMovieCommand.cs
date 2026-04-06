using MediatR;

public class CreateMovieCommand : IRequest<int>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int ReleaseYear { get; set; }
    public required decimal RentalPrice { get; set; }
}