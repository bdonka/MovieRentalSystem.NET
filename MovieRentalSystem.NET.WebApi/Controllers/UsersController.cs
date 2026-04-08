using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetUser(int id)
    {
        var result = await _userService.GetByIdAsync(id);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return Ok(result.Value);
    }

    // POST: api/users
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> PostUser(CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(request);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already exists"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return CreatedAtAction(
            nameof(GetUser),
            new { id = result.Value.Id },
            result.Value);
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutUser(int id, UpdateUserRequest request)
    {
        var result = await _userService.UpdateAsync(id, request);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already exists"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return NoContent();
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}