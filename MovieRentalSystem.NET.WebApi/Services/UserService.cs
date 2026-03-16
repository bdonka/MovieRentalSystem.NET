using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Services;

public class UserService : IUserService
{
    private static readonly List<User> _users = new();

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(_users.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }

    public Task<User> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Id = _users.Count + 1,
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
        };
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Task.FromResult(false);

        user.Name = request.Name;
        user.Email = request.Email;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Task.FromResult(false);
        _users.Remove(user);
        return Task.FromResult(true);
    }





    public Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }
}
