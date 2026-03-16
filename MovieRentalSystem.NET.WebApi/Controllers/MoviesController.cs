using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MoviesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        //return await _context.Movies.ToListAsync();
    }

    // GET: api/movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        //var movie = await _context.Movies.FindAsync(id);
        //if (movie == null)
        //{
        //    return NotFound();
        //}

        //return movie;
    }


    // POST: api/movies
    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(CreateMovieRequest request) 
    {
        //_context.Movies.Add(movie);
        //await _context.SaveChangesAsync();

        //return CreatedAtAction(
        //    nameof(GetMovie),
        //    new { id = movie.Id },
        //    movie);
    }


    // PUT : api/movies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, UpdateMovieRequest request)
    {
        //if (id != movie.Id)
        //{
        //    return BadRequest();
        //}

        //var movieItem = await _context.Movies.FindAsync(id);
        //if (movieItem == null)
        //{
        //    return NotFound();
        //}

        //movieItem.Title = movie.Title;
        //movieItem.Description = movie.Description;
        //movieItem.ReleaseYear = movie.ReleaseYear;
        //movieItem.RentalPrice = movie.RentalPrice;

        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!MovieExists(id))
        //    {
        //        return NotFound();
        //    }
        //    throw;
        //}

        //return NoContent();
    }


    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        //var movie = await _context.Movies.FindAsync(id);
        //if (movie == null)
        //{
        //    return NotFound();
        //}
        //_context.Movies.Remove(movie);
        //await _context.SaveChangesAsync();
        //return NoContent();
    }
    //private bool MovieExists(int id)
    //{
    //        return _context.Movies.Any(e => e.Id == id);
    //}
}

