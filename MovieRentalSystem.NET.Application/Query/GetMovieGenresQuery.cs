using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMovieGenresQuery
: IRequest<IEnumerable<GenreResponse>>
{
    public int MovieId { get; set; }
}

