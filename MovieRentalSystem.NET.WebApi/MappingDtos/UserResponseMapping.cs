using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.MappingDtos;

public static class UserResponseMapping
{
    public static UserResponse MapToUserResponse(this UserDto user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            DateRegistered = user.DateRegistered
        };
    }
}
