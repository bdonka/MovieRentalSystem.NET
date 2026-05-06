using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.WebApi.MappingDtos;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController(IMediator mediator) : ControllerBase
{
    // GET: api/movies
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovieResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMovies([FromQuery] GetMoviesRequest request)
    {
        var movies = await mediator.Send(new GetMovieQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        });
        return Ok(new PagedResponse<MovieResponse>(
            movies.Data.Select(m => m.MapToMovieResponse()).ToList(),
            movies.PageNumber,
            movies.PageSize,
            movies.TotalRecords
        ));
    }   

    // GET: api/movies/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieResponse>> GetMovie(int id)
    {
        var result = await mediator.Send(new GetMovieByIdQuery(id));
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return Ok(result.Value.MapToMovieResponse());
    }

    // POST: api/movies
    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieResponse>> PostMovie(CreateMovieRequest request)
    {
        var result = await mediator.Send(new CreateMovieCommand(
            request.Title,
            request.Description,
            request.ReleaseYear,
            request.RentalPrice
        ));

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
        var result = await mediator.Send(new UpdateMovieCommand(
            id,
            request.Title,
            request.Description,
            request.ReleaseYear,
            request.RentalPrice));

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
        var command = new DeleteMovieCommand { Id = id };
        var result = await mediator.Send(command);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }

    // POST: api/movies/{id}/genres/{id}
    [HttpPost("{movieId}/genres/{genreId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> AssignGenre(int movieId, int genreId)
    {
        var command = new AssignMovieGenreCommand
        {
            MovieId = movieId,
            GenreId = genreId
        };
        var result = await mediator.Send(command);
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
        var command = new RemoveMovieGenreCommand
        {
            MovieId = movieId,
            GenreId = genreId
        };
        var result = await mediator.Send(command);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}