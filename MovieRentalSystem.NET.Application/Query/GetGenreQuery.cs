using MediatR;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetGenreQuery
: PagedQuery, IRequest<PagedResponse<GenreDto>>
{

}

