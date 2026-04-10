using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Query;

public class GetRentalByIdQuery
: IRequest<Result<RentalDto>>
{
    public int Id { get; set; }
}

