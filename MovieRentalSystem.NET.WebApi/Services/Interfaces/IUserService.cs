using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task<bool> UpdateAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
