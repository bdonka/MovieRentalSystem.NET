using FluentResults;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<Result<UserDto>> GetByIdAsync(int id);
        Task<Result<UserDto>> CreateAsync(CreateUserDto request);
        Task<Result> UpdateAsync(int id, UserDto request);
        Task<Result> DeleteAsync(int id);
    }
}
