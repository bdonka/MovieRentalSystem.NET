using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Query;
using MovieRentalSystem.NET.WebApi.Common;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController(IMediator mediator) : ResultsControllerBase
{
    // GET: api/rentals
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<RentalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<RentalDto>>> GetRentals([FromQuery] GetRentalsRequest request)
    {
        var result = await mediator.Send(new GetRentalQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        });
        return Ok(result);
    }

    // GET: api/rentals/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalDto>> GetRental(int id)
    {
        var result = await mediator.Send(new GetRentalByIdQuery { Id = id });
        return ToOkOrErrorResponse(result);
    }

    // POST: api/rentals
    [HttpPost]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentalDto>> PostRental(CreateRentalRequest request)
    {
        var result = await mediator.Send(new CreateRentalCommand
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        });
        return ToCreatedAtActionOrErrorResponse(
            nameof(GetRental),
            new { id = result.Value.Id },
            result);
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
        return ToNoContentOrErrorResponse(result);
    }

    // DELETE: api/rentals/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRental(int id)
    {
        var result = await mediator.Send(new DeleteRentalCommand { Id = id });
        return ToNoContentOrErrorResponse(result);
    }
}