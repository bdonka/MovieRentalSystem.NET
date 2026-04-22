using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public class CreateRentalCommand : IRequest<Result<RentalDto>>
{
    public required int UserId { get; set; }
    public required int MoviePhysicalCopyId { get; set; }
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
}