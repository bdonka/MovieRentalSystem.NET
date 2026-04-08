using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetGenreQuery
: IRequest<IEnumerable<GenreResponse>>
{
}

