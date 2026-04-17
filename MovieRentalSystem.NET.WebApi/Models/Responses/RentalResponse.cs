using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.WebApi.Models.Responses;

public class RentalResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MoviePhysicalCopyId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; } = RentalStatus.Preparing;
}
