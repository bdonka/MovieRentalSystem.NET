using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController(IMediator mediator) : ControllerBase
{
    // GET: api/genres
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GenreResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GenreResponse>>> GetGenres()
    {
        return Ok(await mediator.Send(new GetGenreQuery()));
    }

    // GET: api/genres/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GenreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenreResponse>> GetGenre(int Id)
    {
        return Ok(await mediator.Send(new GetGenreByIdQuery()));
    }


    // POST: api/genres
    [HttpPost]
    [ProducesResponseType(typeof(GenreResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreResponse>> PostGenre(CreateGenreRequest request)
    {
        var result = await _genreService.CreateAsync(request);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already exists"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return CreatedAtAction(nameof(GetGenre), new { id = result.Value.Id }, result.Value);
    }


    // PUT : api/genres/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutGenre(int id, UpdateGenreRequest request)
    {
        var result = await _genreService.UpdateAsync(id, request);
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


    // DELETE: api/genres/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var result = await _genreService.DeleteAsync(id);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("not found"))
                return NotFound(result.Errors.First().Message);
            return BadRequest(result.Errors.First().Message);
        }
        return NoContent();
    }
}
