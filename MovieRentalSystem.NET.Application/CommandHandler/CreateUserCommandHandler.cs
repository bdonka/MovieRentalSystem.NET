using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new CreateUserDto
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        var result = await _userService.CreateAsync(createRequest);

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}