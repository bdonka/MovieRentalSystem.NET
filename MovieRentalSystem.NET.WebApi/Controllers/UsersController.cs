using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Common;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager) : ResultsControllerBase
{
    //POST Register
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, null);
        }
        return BadRequest(result.Errors);
    }

    //POST singInManager
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: request.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Ok();
        }
        return Unauthorized();
    }

    // GET: api/users
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<UserDto>>> GetUsers([FromQuery] GetUsersRequest request)
    {
        var result = await mediator.Send(new GetUserQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,

        });
        return Ok(result);
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        var result = await mediator.Send(new GetUserByIdQuery { Id = id });
        return ToOkOrErrorResponse(result);
    }

    // POST: api/users
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> PostUser(CreateUserRequest request)
    {
        var result = await mediator.Send(new CreateUserCommand
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password
        });
        return ToCreatedAtActionOrErrorResponse(
            nameof(GetUser),
            new { id = result.Value.Id },
            result);
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutUser(string id, UpdateUserRequest request)
    {
        var result = await mediator.Send(new UpdateUserCommand
        {
            Id = id,
            UserName = request.UserName,
            Email = request.Email
        });
        return ToNoContentOrErrorResponse(result);
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await mediator.Send(new DeleteUserCommand { Id = id });
        return ToNoContentOrErrorResponse(result);
    }
}