using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    // GET: api/genres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreResponse>>> GetGenres()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    // GET: api/genres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreResponse>> GetGenre(int id)
    {
        var genre = await _genreService.GetByIdAsync(id);
        if (genre == null) return NotFound();
        return Ok(genre);
    }


    // POST: api/genres
    [HttpPost]
    public async Task<ActionResult<GenreResponse>> PostGenre(CreateGenreRequest request)
    {
        var genre = await _genreService.CreateAsync(request);
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }


    // PUT : api/genres/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenre(int id, UpdateGenreRequest request)
    {
        var updated = await _genreService.UpdateAsync(id, request);
        if (!updated) return NotFound();
        return NoContent();
    }


    // DELETE: api/genres/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var deleted = await _genreService.DeleteAsync(id);
        if(!deleted) return NotFound();
        return NoContent();
    }
}
