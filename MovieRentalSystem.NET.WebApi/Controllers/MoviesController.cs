using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.WebApi.Common;
using MovieRentalSystem.NET.WebApi.Models.Requests.Movies;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController(IMediator mediator) : ResultsControllerBase
{
    // GET: api/movies
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<MovieDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<MovieDto>>> GetMovies([FromQuery] GetMoviesRequest request)
    {
        var result = await mediator.Send(new GetMovieQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        });
        return Ok(result);
    }

    // GET: api/movies/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var result = await mediator.Send(new GetMovieByIdQuery(id));
        return ToOkOrErrorResponse(result);
    }

    // POST: api/movies
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieDto>> PostMovie(CreateMovieRequest request)
    {
        var result = await mediator.Send(new CreateMovieCommand(
            request.Title,
            request.Description,
            request.ReleaseYear,
            request.RentalPrice
        ));
        return ToCreatedAtActionOrErrorResponse(
            nameof(GetMovie),
            new { id = result.Value.Id },
            result
        );
    }

    // PUT : api/movies/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
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
        return ToNoContentOrErrorResponse(result);
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var result = await mediator.Send(new DeleteMovieCommand(id));
        return ToNoContentOrErrorResponse(result);
    }

    // POST: api/movies/{id}/genres/{id}
    [HttpPost("{movieId}/genres/{genreId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> AssignGenre(int movieId, int genreId)
    {
        var command = new AssignMovieGenreCommand
        {
            MovieId = movieId,
            GenreId = genreId
        };
        var result = await mediator.Send(command);
        return ToNoContentOrErrorResponse(result);
    }

    // DELETE: api/movies/{id}/genres/{id}
    [HttpDelete("{movieId}/genres/{genreId}")]
    [Authorize(Roles = "Admin")]
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
        return ToNoContentOrErrorResponse(result);
    }
}