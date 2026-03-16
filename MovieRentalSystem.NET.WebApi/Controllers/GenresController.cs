using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly AppDbContext _context;

    public GenresController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/genres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        //return await _context.Genres.ToListAsync();
    }

    // GET: api/genres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        //var genre = await _context.Genres.FindAsync(id);
        //if (genre == null)
        //{
        //    return NotFound();
        //}

        //return genre;
    }


    // POST: api/genres
    [HttpPost]
    public async Task<ActionResult<Genre>> PostGenre(CreateGenreRequest request)
    {
        //_context.Genres.Add(genre);
        //await _context.SaveChangesAsync();

        //return CreatedAtAction(
        //    nameof(GetGenre),
        //    new { id = genre.Id },
        //    genre);
    }


    // PUT : api/genres/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenre(int id, UpdateGenreRequest request)
    {
        //if (id != genre.Id)
        //{
        //    return BadRequest();
        //}

        //var genreItem = await _context.Genres.FindAsync(id);
        //if (genreItem == null)
        //{
        //    return NotFound();
        //}

        //genreItem.Name = genre.Name;

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


    // DELETE: api/genres/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        //var genre = await _context.Genres.FindAsync(id);
        //if (genre == null)
        //{
        //    return NotFound();
        //}
        //_context.Genres.Remove(genre);
        //await _context.SaveChangesAsync();
        //return NoContent();
    }

    //private bool MovieExists(int id)
    //{
    //    return _context.Movies.Any(e => e.Id == id);
    //}
}
