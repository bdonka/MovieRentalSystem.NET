using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetGenreByIdQuery
: IRequest<GenreResponse>
{
    public int Id { get; set; }
}

