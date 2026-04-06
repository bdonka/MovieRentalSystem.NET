using MediatR;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Query;

public class GenreQuery
: IRequest<List<Genre>>
{
}

