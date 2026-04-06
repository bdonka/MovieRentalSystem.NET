using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class UserMapping
{
    public static UserResponse MapToUserResponse(this User user)
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