using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyByIdQuery
: IRequest<MoviePhysicalCopyResponse>
{
    public int Id { get; set; }
    public int MovieId { get; set; }
}

