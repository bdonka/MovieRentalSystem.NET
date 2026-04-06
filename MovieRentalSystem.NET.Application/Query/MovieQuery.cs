using MediatR;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Query;

public class MovieQuery
: IRequest<List<Movie>>
{
}

