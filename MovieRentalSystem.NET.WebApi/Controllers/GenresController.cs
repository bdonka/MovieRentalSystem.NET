using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.WebApi.Common;
using MovieRentalSystem.NET.WebApi.Models.Requests.Genres;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController(IMediator mediator) : ResultsControllerBase
{
    // GET: api/genres
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<GenreDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<GenreDto>>> GetGenres([FromQuery] GetGenresRequest request) 
    {
        var result = await mediator.Send(new GetGenreQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        });
        return Ok(result);
    }

    // GET: api/genres/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenreDto>> GetGenre(int id)
    {
        var result = await mediator.Send(new GetGenreByIdQuery
        {
            Id = id
        });
        return ToOkOrErrorResponse(result);
    }


    // POST: api/genres
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreDto>> PostGenre(CreateGenreRequest request)
    {
        var result = await mediator.Send(new CreateGenreCommand()
        {
            Name = request.Name
        });

        return ToCreatedAtActionOrErrorResponse(nameof(GetGenre), new { id = result.Value.Id }, result);
    }


    // PUT : api/genres/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutGenre(int id, UpdateGenreRequest request)
    {
        var result = await mediator.Send(new UpdateGenreCommand
        {
            Id = id,
            Name = request.Name
        });

        return ToNoContentOrErrorResponse(result);
    }


    // DELETE: api/genres/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var result = await mediator.Send(new DeleteGenreCommand { Id = id });
        return ToNoContentOrErrorResponse(result);
    }
}
