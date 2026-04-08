using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetUserQuery
: IRequest<IEnumerable<UserResponse>>
{
}

