using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.WebApi.MappingDtos;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController(IMediator mediator) : ControllerBase
{
    // GET: api/rentals
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RentalResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RentalResponse>>> GetRentals([FromQuery] GetRentalsRequest request)
    {
        var rentals = await mediator.Send(new GetRentalQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        });
        return Ok(rentals);
    }

    // GET: api/rentals/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalResponse>> GetRental(int id)
    {
        var result = await mediator.Send(new GetRentalByIdQuery { Id = id });
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return Ok(result.Value);
    }

    // POST: api/rentals
    [HttpPost]
    [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentalResponse>> PostRental(CreateRentalRequest request)
    {
        var result = await mediator.Send(new CreateRentalCommand
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        });
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already rented"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return CreatedAtAction(
            nameof(GetRental),
            new { id = result.Value.Id },
            result.Value.MapToRentalResponse());
    }

    // PUT: api/rentals/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutRental(int id, UpdateRentalRequest request)
    {
        var result = await mediator.Send(new UpdateRentalCommand
        {
            Id = id,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate,
            ReturnDate = request.ReturnDate,
            TotalPrice = request.TotalPrice,
            Status = request.Status
        });
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already rented"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return NoContent();
    }

    // DELETE: api/rentals/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRental(int id)
    {
        var result = await mediator.Send(new DeleteRentalCommand { Id = id });
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}