using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Infrastructure.Mapping;

public static class UserMapping
{
    public static CreateUserDto MapToCreateUserDto(this User user)
    {
        return new CreateUserDto
        {
            Name = user.Name,
            Email = user.Email ?? throw new ArgumentNullException(nameof(user.Email)),
            Password = user.Password ?? throw new ArgumentNullException(nameof(user.Password))
        };
    }
    public static UserDto MapToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            DateRegistered = user.DateRegistered
        };
    }

    public static User MapToUserEntity(this UserDto user)
    {
        return new User
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            DateRegistered = user.DateRegistered
        };
    }
}
