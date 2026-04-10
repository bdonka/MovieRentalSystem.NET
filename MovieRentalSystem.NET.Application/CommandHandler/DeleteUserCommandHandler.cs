using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(
        DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteAsync(request.Id);
        if (result.IsFailed)
        {
            throw new ApplicationException(result.Errors.First().Message);
        }

        return result;
    }
}