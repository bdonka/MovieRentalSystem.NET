using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Entities;

namespace MovieRentalSystem.NET.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RentalsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/rentals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
    {
        return await _context.Rentals
            .Include(r=> r.User)
            .Include(r=>r.MoviePhysicalCopy)
            .ToListAsync();
    }

    // GET: api/rental/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetRental(int id)
    {
        var rental = await _context.Rentals.FindAsync(id);
        if (rental == null)
        {
            return NotFound();
        }

        return rental;
    }


    // POST: api/rentals
    [HttpPost]
    public async Task<ActionResult<Rental>> PostRental(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetRental),
            new { id = rental.Id },
            rental);
    }


    // PUT : api/rentals/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRental(int id, Rental rental)
    {
        if (id != rental.Id)
        {
            return BadRequest();
        }

        var rentalItem = await _context.Rentals.FindAsync(id);
        if (rentalItem == null)
        {
            return NotFound();
        }

        rentalItem.OrderDate = rental.OrderDate;
        rentalItem.RentalStartDate = rental.RentalStartDate;
        rentalItem.DueDate = rental.DueDate;
        rentalItem.ReturnDate = rental.ReturnDate;
        rentalItem.TotalPrice = rental.TotalPrice;
        rentalItem.Status = rental.Status;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RentalExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }


    // DELETE: api/rental/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRental(int id)
    {
        var rental = await _context.Rentals.FindAsync(id);
        if (rental == null)
        {
            return NotFound();
        }
        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    private bool RentalExists(int id)
    {
        return _context.Rentals.Any(e => e.Id == id);
    }
}
