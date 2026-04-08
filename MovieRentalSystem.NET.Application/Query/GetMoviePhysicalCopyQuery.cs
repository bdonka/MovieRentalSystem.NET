using MediatR;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyQuery
: IRequest<IEnumerable<MoviePhysicalCopyResponse>>
{
}

