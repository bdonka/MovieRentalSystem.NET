using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;

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
    public async Task<ActionResult<IEnumerable<RentalResponse>>> GetRentals()
    {
        var rentals = await _rentalService.GetAllAsync();
        return Ok(rentals);
    }

    // GET: api/rentals/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RentalResponse>> GetRental(int id)
    {
        var rental = await _rentalService.GetByIdAsync(id);
        if (rental == null) return NotFound();
        return Ok(rental);
    }

    // POST: api/rentals
    [HttpPost]
    public async Task<ActionResult<RentalResponse>> PostRental(CreateRentalRequest request)
    {
        var rental = await _rentalService.CreateAsync(request);
        return CreatedAtAction(
            nameof(GetRental),
            new { id = rental.Id },
            rental);
    }

    // PUT: api/rentals/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRental(int id, UpdateRentalRequest request)
    {
        var updated = await _rentalService.UpdateAsync(id, request);
        if (!updated) return NotFound();
        return NoContent();
    }

    // DELETE: api/rentals/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRental(int id)
    {
        var deleted = await _rentalService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}