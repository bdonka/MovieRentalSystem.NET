using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetMovieByIdQuery
: IRequest<MovieResponse>
{
    public int Id { get; set; }
}

