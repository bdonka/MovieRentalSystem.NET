using MediatR;

public class CreateGenreCommand : IRequest<int>
{
    public required string Name { get; set; }
}