using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviePhysicalCopiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MoviePhysicalCopiesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/moviePhysicalCopies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MoviePhysicalCopy>>> GetMoviePhysicalCopies()
    {
        //return await _context.MoviePhysicalCopies.ToListAsync();
    }

    // GET: api/movies/id
    [HttpGet("{id}")]
    public async Task<ActionResult<MoviePhysicalCopy>> GetMoviePhysicalCopy(int id)
    {
        //var moviePhysicalCopy = await _context.MoviePhysicalCopies.FindAsync(id);
        //if (moviePhysicalCopy == null)
        //{
        //    return NotFound();
        //}

        //return moviePhysicalCopy;
    }


    // POST: api/moviePhysicalCopy
    [HttpPost]
    public async Task<ActionResult<MoviePhysicalCopy>> PostMoviePhysicalCopy(CreateMoviePhysicalCopyRequest request)
    {
        //var exists = await _context.MoviePhysicalCopies
        //    .AnyAsync(mg => mg.Id == moviePhysicalCopy.Id);

        //if (exists)
        //{
        //    return Conflict("This physical copy already exists.");
        //}

        //_context.MoviePhysicalCopies.Add(moviePhysicalCopy);
        //await _context.SaveChangesAsync();

        //return CreatedAtAction(
        //    nameof(GetMoviePhysicalCopy),
        //    new { id = moviePhysicalCopy.Id },
        //    moviePhysicalCopy);
    }

    // PUT: api/moviePhysicalCopies/{id}/{movieId}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMoviePhysicalCopy(int id, UpdateMoviePhysicalCopyRequest request)
    {
        //if (id != moviePhysicalCopy.Id)
        //{
        //    return BadRequest();
        //}

        //var existingCopy = await _context.MoviePhysicalCopies.FindAsync(id);

        //if (existingCopy == null)
        //{
        //    return NotFound();
        //}

        //existingCopy.Status = moviePhysicalCopy.Status;

        //await _context.SaveChangesAsync();

        //return NoContent();
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMoviePhysicalCopy(int id)
    {
        //var moviePhysicalCopy = await _context.MoviePhysicalCopies.FindAsync(id);
        //if (moviePhysicalCopy == null)
        //{
        //    return NotFound();
        //}
        //_context.MoviePhysicalCopies.Remove(moviePhysicalCopy);
        //await _context.SaveChangesAsync();
        //return NoContent();
    }
}
