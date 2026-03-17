using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieGenresController : ControllerBase
{
    private readonly IMovieGenreService _movieGenreService;

    public MovieGenresController(IMovieGenreService movieGenreService)
    {
        _movieGenreService = movieGenreService;
    }

    // GET: api/movieGenres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieGenreResponse>>> GetMovieGenres()
    {
        var movieGenres = await _movieGenreService.GetAllAsync();
        return Ok(movieGenres);
    }

    // GET: api/movieGenres/{movieId}/{genreId}
    [HttpGet("{movieId}/{genreId}")]
    public async Task<ActionResult<MovieGenreResponse>> GetMovieGenre(int movieId, int genreId)
    {
        var movieGenre = await _movieGenreService.GetByIdAsync(movieId, genreId);
        if (movieGenre == null) return NotFound();
        return Ok(movieGenre);
    }

    // POST: api/movieGenres
    [HttpPost]
    public async Task<ActionResult<MovieGenreResponse>> PostMovieGenre(CreateMovieGenreRequest request)
    {
        var movieGenre = await _movieGenreService.CreateAsync(request);
        return CreatedAtAction(
            nameof(GetMovieGenre),
            new { movieId = movieGenre.MovieId, genreId = movieGenre.GenreId },
            movieGenre);
    }

    // DELETE: api/movieGenres/{movieId}/{genreId}
    [HttpDelete("{movieId}/{genreId}")]
    public async Task<IActionResult> DeleteMovieGenre(int movieId, int genreId)
    {
        var deleted = await _movieGenreService.DeleteAsync(movieId, genreId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}