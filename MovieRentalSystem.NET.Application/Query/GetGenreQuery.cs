using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetGenreQuery
: IRequest<IEnumerable<GenreDto>>
{
}

