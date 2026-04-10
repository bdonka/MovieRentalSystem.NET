using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRentalSystem.NET.Infrastructure.Mapping;

public static class RentalMapping
{
    public static RentalDto MapToRentalDto(this Rental rental)
    {
        return new RentalDto
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

    public static Rental MapToRentalEntity(this RentalDto rental)
    {
        return new Rental
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
