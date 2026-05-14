using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public class CreateUserCommand : IRequest<Result<UserDto>>
{
    public required string UserName { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
}