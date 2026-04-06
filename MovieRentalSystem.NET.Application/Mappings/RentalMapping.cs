using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class RentalMapping
{
    public static RentalResponse MapToRentalResponse(this Rental rental)
    {
        return new RentalResponse
        {
            Id = rental.Id,
            UserId = rental.UserId,
            MoviePhysicalCopyId = rental.MoviePhysicalCopyId,
            OrderDate = rental.OrderDate,
            RentalStartDate = rental.RentalStartDate,
            DueDate = rental.DueDate,
            ReturnDate = rental.ReturnDate,
            TotalPrice = rental.TotalPrice,
            Status = rental.Status
        };
    }
}