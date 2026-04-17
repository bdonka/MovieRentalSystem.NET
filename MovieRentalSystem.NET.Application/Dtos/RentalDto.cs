using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Application.Dtos;

public class RentalDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserDto User { get; set; } = null!;
    public int MoviePhysicalCopyId { get; set; }
    public MoviePhysicalCopyDto MoviePhysicalCopy { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
}
