using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMovieQuery
: IRequest<IEnumerable<MovieResponse>>
{
}

