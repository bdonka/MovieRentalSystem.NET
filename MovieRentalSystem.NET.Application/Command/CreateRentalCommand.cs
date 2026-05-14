using FluentResults;
using MediatR;
using MovieRentalSystem.NET.Application.Dtos;

public class CreateRentalCommand : IRequest<Result<RentalDto>>
{
    public required string UserId { get; set; } //string by Identity
    public required int MoviePhysicalCopyId { get; set; }
    public DateTime? RentalStartDate { get; set; }
    public DateTime? DueDate { get; set; }
}