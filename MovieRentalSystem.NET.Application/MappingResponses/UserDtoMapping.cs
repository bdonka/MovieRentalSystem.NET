using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class UserDtoMapping
{
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
}