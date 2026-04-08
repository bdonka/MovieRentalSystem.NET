using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetRentalQuery
: IRequest<IEnumerable<RentalResponse>>
{
}

