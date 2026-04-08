using MediatR;

namespace MovieRentalSystem.NET.Application.Query;

public class GetUserByIdQuery
: IRequest<UserResponse>
{
    public int Id { get; set; }
}

