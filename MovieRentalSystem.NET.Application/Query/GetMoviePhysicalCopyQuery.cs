using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyQuery
: IRequest<IEnumerable<MoviePhysicalCopyDto>>
{
}

