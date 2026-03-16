namespace MovieRentalSystem.NET.WebApi.Models.Requests.Rentals;

public class CreateRentalRequest
{
    public int UserId { get; set; }
    public int MoviePhysicalCopyId { get; set; }
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
}
