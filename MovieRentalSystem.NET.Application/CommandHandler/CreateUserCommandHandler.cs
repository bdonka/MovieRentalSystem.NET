using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createRequest = new CreateUserRequest
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

        return result.Value.Id;
    }
}