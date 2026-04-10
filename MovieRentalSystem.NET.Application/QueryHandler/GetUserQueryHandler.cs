using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IEnumerable<UserDto>>
{
    private readonly IUserService _userService;
    public GetUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetAllAsync();
    }
}