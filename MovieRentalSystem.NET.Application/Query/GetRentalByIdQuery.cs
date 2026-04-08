using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetRentalByIdQuery
: IRequest<RentalResponse>
{
    public int Id { get; set; }
}

