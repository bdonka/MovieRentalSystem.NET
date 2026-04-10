using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IMediator _mediator;
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
        var result = await _movieService.GetByIdAsync(id);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return Ok(result.Value);
    }

    // POST: api/movies
    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieResponse>> PostMovie(CreateMovieRequest request)
    {
       var result = await _mediator.Send(new CreateMovieCommand())

        var result = await _movieService.CreateAsync(request);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already exists"))
                return Conflict(result.Errors.First().Message);
            return BadRequest(result.Errors.First().Message);
        }
        return CreatedAtAction(
            nameof(GetMovie),
            new { id = result.Value.Id },
            result.Value);
    }

    // PUT : api/movies/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutMovie(int id, UpdateMovieRequest request)
    {
        var result = await _movieService.UpdateAsync(id, request);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("not found"))
                return NotFound(result.Errors.First().Message);

            if (result.Errors.First().Message.Contains("already exists"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return NoContent();
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var result = await _movieService.DeleteAsync(id);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }


    // GET: /api/movies/{id}/genres
    [HttpGet("{movieId}/genres")]
    [ProducesResponseType(typeof(IEnumerable<GenreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<GenreResponse>>> GetGenres(int movieId)
    {
        var result = await _movieService.GetGenresAsync(movieId);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return Ok(result.Value);
    }

    // POST: api/movies/{id}/genres/{id}
    [HttpPost("{movieId}/genres/{genreId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> AssignGenre(int movieId, int genreId)
    {
        var result = await _movieService.AssignGenreAsync(movieId, genreId);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }

    // DELETE: api/movies/{id}/genres/{id}
    [HttpDelete("{movieId}/genres/{genreId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveGenre(int movieId, int genreId)
    {
        var result = await _movieService.RemoveGenreAsync(movieId, genreId);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}