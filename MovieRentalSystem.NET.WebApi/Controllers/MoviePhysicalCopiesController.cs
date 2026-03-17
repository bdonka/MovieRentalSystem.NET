using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviePhysicalCopiesController : ControllerBase
{
    private readonly IMoviePhysicalCopyService _copyService;

    public MoviePhysicalCopiesController(IMoviePhysicalCopyService copyService)
    {
        _copyService = copyService;
    }

    // GET: api/moviePhysicalCopies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MoviePhysicalCopyResponse>>> GetMoviePhysicalCopies()
    {
        var copies = await _copyService.GetAllAsync();
        return Ok(copies);
    }

    // GET: api/moviePhysicalCopies/{id}/{movieId}
    [HttpGet("{id}/{movieId}")]
    public async Task<ActionResult<MoviePhysicalCopyResponse>> GetMoviePhysicalCopy(int id, int movieId)
    {
        var copy = await _copyService.GetByIdAsync(id, movieId);
        if (copy == null) return NotFound();
        return Ok(copy);
    }

    // POST: api/moviePhysicalCopies
    [HttpPost]
    public async Task<ActionResult<MoviePhysicalCopyResponse>> PostMoviePhysicalCopy(CreateMoviePhysicalCopyRequest request)
    {
        var copy = await _copyService.CreateAsync(request);
        return CreatedAtAction(
            nameof(GetMoviePhysicalCopy),
            new { id = copy.Id, movieId = copy.MovieId },
            copy);
    }

    // PUT: api/moviePhysicalCopies/{id}/{movieId}
    [HttpPut("{id}/{movieId}")]
    public async Task<IActionResult> PutMoviePhysicalCopy(int id, int movieId, UpdateMoviePhysicalCopyRequest request)
    {
        var updated = await _copyService.UpdateAsync(id, movieId, request);
        if (!updated) return NotFound();
        return NoContent();
    }

    // DELETE: api/moviePhysicalCopies/{id}/{movieId}
    [HttpDelete("{id}/{movieId}")]
    public async Task<IActionResult> DeleteMoviePhysicalCopy(int id, int movieId)
    {
        var deleted = await _copyService.DeleteAsync(id, movieId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}