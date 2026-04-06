using FluentResults;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<Result<UserResponse>> GetByIdAsync(int id);
        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request);
        Task<Result> UpdateAsync(int id, UpdateUserRequest request);
        Task<Result> DeleteAsync(int id);
    }
}
