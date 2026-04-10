using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Domain.Enums;

public class UpdateRentalCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal TotalPrice { get; set; }
    public required RentalStatus Status {  get; set; } = RentalStatus.Preparing;
}