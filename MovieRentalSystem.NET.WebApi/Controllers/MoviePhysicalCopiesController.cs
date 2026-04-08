using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

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
    [ProducesResponseType(typeof(IEnumerable<MoviePhysicalCopyResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MoviePhysicalCopyResponse>>> GetMoviePhysicalCopies()
    {
        var copies = await _copyService.GetAllAsync();
        return Ok(copies);
    }

    // GET: api/moviePhysicalCopies/{id}/{movieId}
    [HttpGet("{id}/{movieId}")]
    [ProducesResponseType(typeof(MoviePhysicalCopyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MoviePhysicalCopyResponse>> GetMoviePhysicalCopy(int id, int movieId)
    {
        var result = await _copyService.GetByIdAsync(id, movieId);
        if (result.IsFailed) 
            return NotFound(result.Errors.First().Message);
        return Ok(result.Value);
    }

    // POST: api/moviePhysicalCopies
    [HttpPost]
    [ProducesResponseType(typeof(MoviePhysicalCopyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MoviePhysicalCopyResponse>> PostMoviePhysicalCopy(CreateMoviePhysicalCopyRequest request)
    {
        var result = await _copyService.CreateAsync(request);
        if (result.IsFailed) 
            return BadRequest(result.Errors.First().Message);
        return CreatedAtAction(
            nameof(GetMoviePhysicalCopy),
            new { id = result.Value.Id, movieId = result.Value.MovieId },
            result.Value);
    }

    // PUT: api/moviePhysicalCopies/{id}/{movieId}
    [HttpPut("{id}/{movieId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutMoviePhysicalCopy(int id, int movieId, UpdateMoviePhysicalCopyRequest request)
    {
        var result = await _copyService.UpdateAsync(id, movieId, request);
        if (result.IsFailed) 
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }

    // DELETE: api/moviePhysicalCopies/{id}/{movieId}
    [HttpDelete("{id}/{movieId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMoviePhysicalCopy(int id, int movieId)
    {
        var result = await _copyService.DeleteAsync(id, movieId);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}