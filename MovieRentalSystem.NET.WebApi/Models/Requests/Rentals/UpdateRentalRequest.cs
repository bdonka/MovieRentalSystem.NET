using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;

public class UpdateRentalRequest
{
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; }
}
