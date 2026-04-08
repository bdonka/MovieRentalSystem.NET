using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.WebApi.Models.Responses;

namespace MovieRentalSystem.NET.WebApi.MappingDtos;

public static class RentalResponseMapping
{
    public static RentalResponse MapToRentalResponse(this RentalDto rental)
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
