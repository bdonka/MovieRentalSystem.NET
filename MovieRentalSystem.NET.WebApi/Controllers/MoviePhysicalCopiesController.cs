using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.WebApi.MappingDtos;
using MovieRentalSystem.NET.WebApi.Models.Requests.MoviePhysicalCopies;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviePhysicalCopiesController(IMediator mediator) : ControllerBase
{

    // GET: api/moviePhysicalCopies
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MoviePhysicalCopyResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MoviePhysicalCopyResponse>>> GetMoviePhysicalCopies([FromQuery] GetMoviePhysicalCopiesRequest request)
    {
        var result = await mediator.Send(new GetMoviePhysicalCopyQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        });
        return Ok(new PagedResponse<MoviePhysicalCopyResponse>(
            result.Data.Select(r => r.MapToMoviePhysicalCopyResponse()).ToList(),
            result.PageNumber,
            result.PageSize,
            result.TotalRecords));
    }

    // GET: api/moviePhysicalCopies/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MoviePhysicalCopyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MoviePhysicalCopyResponse>> GetMoviePhysicalCopy(int id)
    {
        var result = await mediator.Send(new GetMoviePhysicalCopyByIdQuery { Id = id });
        if (result.IsFailed) 
            return NotFound(result.Errors.First().Message);
        return Ok(result);
    }

    // POST: api/moviePhysicalCopies
    [HttpPost]
    [ProducesResponseType(typeof(MoviePhysicalCopyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MoviePhysicalCopyResponse>> PostMoviePhysicalCopy(CreateMoviePhysicalCopyRequest request)
    {
        var result = await mediator.Send(new CreateMoviePhysicalCopyCommand
        {
            MovieId = request.MovieId,
            Code = request.Code
        });
        if (result.IsFailed) 
            return BadRequest(result.Errors.First().Message);
        return CreatedAtAction(
            nameof(GetMoviePhysicalCopy),
            new { id = result.Value.Id },
            result.Value.MapToMoviePhysicalCopyResponse());
    }

    // PUT: api/moviePhysicalCopies/{id}/{movieId}
    [HttpPut("{id}/{movieId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutMoviePhysicalCopy(int id, int movieId, UpdateMoviePhysicalCopyRequest request)
    {
        var result = await mediator.Send(new UpdateMoviePhysicalCopyCommand
        {
            Id = id,
            MovieId = movieId,
            Status = request.Status
        });
        if (result.IsFailed) 
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }

    // DELETE: api/moviePhysicalCopies/{id}/{movieId}
    [HttpDelete("{id}/{movieId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMoviePhysicalCopy(int id)
    {
        var result = await mediator.Send(new DeleteMoviePhysicalCopyCommand { Id = id});
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}