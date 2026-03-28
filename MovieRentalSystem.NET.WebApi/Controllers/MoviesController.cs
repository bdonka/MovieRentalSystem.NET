using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/movies
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovieResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMovies()
    {
        var movies = await _movieService.GetAllAsync();
        return Ok(movies);
    }

    // GET: api/movies/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieResponse>> GetMovie(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        if (movie == null) return NotFound();
        return Ok(movie);
    }

    // POST: api/movies
    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieResponse>> PostMovie(CreateMovieRequest request)
    {
        var movie = await _movieService.CreateAsync(request);
        return CreatedAtAction(
            nameof(GetMovie),
            new { id = movie.Id },
            movie);
    }

    // PUT : api/movies/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutMovie(int id, UpdateMovieRequest request)
    {
        var updated = await _movieService.UpdateAsync(id, request);
        if (!updated) return NotFound();
        return NoContent();
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var deleted = await _movieService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}