using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MovieGenres;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieGenresController : ControllerBase
{
    private readonly AppDbContext _context;

    public MovieGenresController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/movieGenres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieGenre>>> GetMovieGenres()
    {
        //return await _context.MovieGenres.ToListAsync();
    }

    // GET: api/movieGenres/{movieId}/{genreId}
    [HttpGet("{movieId}/{genreId}")]
    public async Task<ActionResult<MovieGenre>> GetMovieGenre(int movieId, int genreId)
    {
        //var movieGenre = await _context.MovieGenres.FindAsync(movieId, genreId);
        //if (movieGenre == null)
        //{
        //    return NotFound();
        //}

        //return movieGenre;
    }


    // POST: api/movieGenres
    [HttpPost]
    public async Task<ActionResult<MovieGenre>> PostMovieGenre(CreateMovieGenreRequest request)
    {
        //var exists = await _context.MovieGenres
        //    .AnyAsync(mg => mg.MovieId == movieGenre.MovieId && mg.GenreId == movieGenre.GenreId);

        //if (exists)
        //{
        //    return Conflict("This movie already has this genre.");
        //}

        //_context.MovieGenres.Add(movieGenre);
        //await _context.SaveChangesAsync();

        //return CreatedAtAction(
        //    nameof(GetMovieGenre),
        //    new { movieId = movieGenre.MovieId, genreId = movieGenre.GenreId },
        //    movieGenre);
    }


    // DELETE: api/movieGenres/{movieId}/{genreId}
    [HttpDelete("{movieId}/{genreId}")]
    public async Task<IActionResult> DeleteMovieGenre(int movieId, int genreId)
    {
        //var movieGenre = await _context.MovieGenres.FindAsync(movieId, genreId);
        //if (movieGenre == null)
        //{
        //    return NotFound();
        //}
        //_context.MovieGenres.Remove(movieGenre);
        //await _context.SaveChangesAsync();
        //return NoContent();
    }
}
