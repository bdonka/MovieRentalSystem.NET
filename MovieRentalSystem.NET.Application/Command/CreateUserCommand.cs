using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public class CreateUserCommand : IRequest<Result<UserDto>>
{
    public required string Name { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}