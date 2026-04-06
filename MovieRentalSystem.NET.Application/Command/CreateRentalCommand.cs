using MediatR;

public class CreateRentalCommand : IRequest<int>
{
    public required int UserId { get; set; }
    public required int MoviePhysicalCopyId { get; set; }
    public required DateTime? RentalStartDate { get; set; }
    public required DateTime? DueDate { get; set; }
}