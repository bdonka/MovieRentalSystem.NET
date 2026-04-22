using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetUserByIdQuery
: IRequest<Result<UserDto>>
{
    public int Id { get; set; }
}

