using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    // GET: api/rentals
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RentalResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RentalResponse>>> GetRentals()
    {
        var rentals = await _rentalService.GetAllAsync();
        return Ok(rentals);
    }

    // GET: api/rentals/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalResponse>> GetRental(int id)
    {
        var result = await _rentalService.GetByIdAsync(id);
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
        var result = await _rentalService.CreateAsync(request);
        if (result.IsFailed)
        {
            if (result.Errors.First().Message.Contains("already rented"))
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
        return CreatedAtAction(
            nameof(GetRental),
            new { id = result.Value.Id },
            result.Value);
    }

    // PUT: api/rentals/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutRental(int id, UpdateRentalRequest request)
    {
        var result = await _rentalService.UpdateAsync(id, request);
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
        var result = await _rentalService.DeleteAsync(id);
        if (result.IsFailed)
            return NotFound(result.Errors.First().Message);
        return NoContent();
    }
}