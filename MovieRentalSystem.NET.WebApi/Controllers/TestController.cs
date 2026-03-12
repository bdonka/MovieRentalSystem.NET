using Microsoft.AspNetCore.Mvc;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[ApiController]
[Route("api")]
public class TestController : ControllerBase
{
    // GET: api/hello
    [HttpGet("hello")]
    public IActionResult GetHello()
    {
        return Ok("Hello World!");
    }
}
