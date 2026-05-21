using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class UserMapping
{
    public static UserDto MapToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email,
            DateRegistered = user.DateRegistered,
            Rentals = user.Rentals?.Select(r => r.MapToRentalDto()).ToList() ?? new List<RentalDto>()
        };
    }
}
