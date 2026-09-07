using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // POST: /auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return Ok();
    }

    // POST: /auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        Console.WriteLine($"LOGIN: looking for user {request.Email}");

        var user = await _userManager.FindByEmailAsync(request.Email);

        Console.WriteLine(
            $"LOGIN: user found = {user is not null}");

        if (user is null)
        {
            return Unauthorized();
        }

        Console.WriteLine("LOGIN: before PasswordSignInAsync");

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!,
            request.Password,
            request.RememberMe,
            lockoutOnFailure: false);

        Console.WriteLine(
            $"LOGIN: PasswordSignInAsync finished. Succeeded={result.Succeeded}");

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        Console.WriteLine("LOGIN: returning OK");

        return Ok();
    }

    // POST: /auth/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    // GET: /auth/me
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!
        };

        return Ok(dto);
    }
}
