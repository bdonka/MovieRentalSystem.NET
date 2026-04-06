using MediatR;
public class CreateMoviePhysicalCopyCommand : IRequest<int>
{
    public required int MovieId { get; set; }
    public required string Code { get; set; } = null!;
}