using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(
        UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new UserDto
        {
            Name = request.Name,
            Email = request.Email,
        };
        var result = await _userService.UpdateAsync(request.Id, updateRequest);
        return result;
    }
}