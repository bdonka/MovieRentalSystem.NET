using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;

public class UserQueryHandler : IRequestHandler<UserQuery, List<User>>
{
    private readonly IUserService _userService;
    public UserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<User>> Handle(
        UserQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllAsync();

        return result.Select(r => new User
        {
            Id = r.Id,
            Name = r.Name,
            Email = r.Email,
        }).ToList();
    }
}